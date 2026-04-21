using Microsoft.EntityFrameworkCore.Migrations; // Import the Entity Framework Core migrations namespace for database schema changes

#nullable disable // Disable nullable reference type checking for this specific file to avoid compiler warnings

namespace ECommercePlatform.Data.Migrations // Define the namespace where this migration class resides
{ // Start of the namespace block
    /// <inheritdoc /> // Documentation tag indicating that comments are inherited from the base Migration class
    public partial class InitialCreate : Migration // Define a partial class named InitialCreate that inherits from the base Migration class
    { // Start of the class block
        /// <inheritdoc /> // Inherit documentation for the Up method
        protected override void Up(MigrationBuilder migrationBuilder) // Method used to apply changes (create tables, etc.) to the database
        { // Start of the Up method body

        } // End of the Up method body

        /// <inheritdoc /> // Inherit documentation for the Down method
        protected override void Down(MigrationBuilder migrationBuilder) // Method used to revert the changes made in the Up method
        { // Start of the Down method body

        } // End of the Down method body
    } // End of the class block
} // End of the namespace block