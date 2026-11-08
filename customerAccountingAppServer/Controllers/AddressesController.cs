using customerAccountingAppServer.Classes;
using customerAccountingAppServer.Classes.databaseTables;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace customerAccountingAppServer.Controllers
{
    [ApiController]
    [Route("api/addresses")]
    public class AddressesController : ControllerBase
    {
        private readonly DataBaseConnection _connection;

        public AddressesController(DataBaseConnection connection)
        {
            _connection = connection;
        }

        [HttpPost("ensure")]
        public async Task<ActionResult> EnsureAddress([FromBody] EnsureAddressDto data)
        {
            if (string.IsNullOrWhiteSpace(data.City) ||
                string.IsNullOrWhiteSpace(data.Street) ||
                string.IsNullOrWhiteSpace(data.HouseNumber) ||
                string.IsNullOrWhiteSpace(data.OrganizationName))
            {
                return BadRequest(new
                {
                    message = "Укажите город, улицу, дом и управляющую компанию."
                });
            }

            string city = data.City.Trim();
            string street = data.Street.Trim();
            string houseNumber = data.HouseNumber.Trim();
            string organizationName = data.OrganizationName.Trim();

            await using NpgsqlConnection con = _connection.getConnection();
            await con.OpenAsync();
            await using NpgsqlTransaction transaction = await con.BeginTransactionAsync();

            try
            {
                int organizationId = 0;

                await using (NpgsqlCommand organizationCommand = new NpgsqlCommand("""
                    SELECT id
                    FROM organizations
                    WHERE name = @name
                    LIMIT 1
                    """, con, transaction))
                {
                    organizationCommand.Parameters.AddWithValue("@name", organizationName);

                    object? organizationResult = await organizationCommand.ExecuteScalarAsync();

                    if (organizationResult is not null && organizationResult != DBNull.Value)
                    {
                        organizationId = Convert.ToInt32(organizationResult);
                    }
                }

                if (organizationId == 0)
                {
                    await using NpgsqlCommand insertOrganizationCommand = new NpgsqlCommand("""
                        INSERT INTO organizations (name)
                        VALUES (@name)
                        ON CONFLICT (name) DO UPDATE SET name = EXCLUDED.name
                        RETURNING id
                        """, con, transaction);
                    insertOrganizationCommand.Parameters.AddWithValue("@name", organizationName);

                    object? organizationResult = await insertOrganizationCommand.ExecuteScalarAsync();
                    organizationId = Convert.ToInt32(organizationResult);
                }

                int addressId = 0;

                await using (NpgsqlCommand addressCommand = new NpgsqlCommand("""
                    SELECT id
                    FROM addresses
                    WHERE city = @city
                      AND street = @street
                      AND house_number = @house_number
                    LIMIT 1
                    """, con, transaction))
                {
                    addressCommand.Parameters.AddWithValue("@city", city);
                    addressCommand.Parameters.AddWithValue("@street", street);
                    addressCommand.Parameters.AddWithValue("@house_number", houseNumber);

                    object? addressResult = await addressCommand.ExecuteScalarAsync();

                    if (addressResult is not null && addressResult != DBNull.Value)
                    {
                        addressId = Convert.ToInt32(addressResult);
                    }
                }

                if (addressId == 0)
                {
                    await using NpgsqlCommand insertAddressCommand = new NpgsqlCommand("""
                        INSERT INTO addresses (
                            city,
                            street,
                            house_number,
                            organization_id
                        )
                        VALUES (
                            @city,
                            @street,
                            @house_number,
                            @organization_id
                        )
                        RETURNING id
                        """, con, transaction);

                    insertAddressCommand.Parameters.AddWithValue("@city", city);
                    insertAddressCommand.Parameters.AddWithValue("@street", street);
                    insertAddressCommand.Parameters.AddWithValue("@house_number", houseNumber);
                    insertAddressCommand.Parameters.AddWithValue("@organization_id", organizationId);

                    object? addressResult = await insertAddressCommand.ExecuteScalarAsync();
                    addressId = Convert.ToInt32(addressResult);
                }
                else
                {
                    await using NpgsqlCommand updateAddressCommand = new NpgsqlCommand("""
                        UPDATE addresses
                        SET organization_id = @organization_id
                        WHERE id = @id
                        """, con, transaction);

                    updateAddressCommand.Parameters.AddWithValue("@organization_id", organizationId);
                    updateAddressCommand.Parameters.AddWithValue("@id", addressId);

                    await updateAddressCommand.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();

                return Ok(new { addressId });
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

}
