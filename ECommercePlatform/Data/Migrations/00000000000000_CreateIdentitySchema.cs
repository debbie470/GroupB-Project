using System; // Import the base System namespace for core types like DateTimeOffset
using Microsoft.EntityFrameworkCore.Metadata; // Import metadata types for database schema configuration
using Microsoft.EntityFrameworkCore.Migrations; // Import migration-specific types and methods

namespace ECommercePlatform.Data.Migrations // Define the namespace where migrations are stored
{ // Start of namespace
    public partial class CreateIdentitySchema : Migration // Define a partial class that inherits from the Migration base class
    { // Start of class
        protected override void Up(MigrationBuilder migrationBuilder) // Method that defines changes to be applied to the database
        { // Start of Up method
            migrationBuilder.CreateTable( // Instruction to create a new database table
                name: "AspNetRoles", // Name of the table for user roles
                columns: table => new // Define the columns for this table
                { // Start of columns definition
                    Id = table.Column<string>(nullable: false), // Unique identifier for the role (Primary Key)
                    Name = table.Column<string>(maxLength: 256, nullable: true), // The display name of the role
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true), // Upper-cased name for efficient searching
                    ConcurrencyStamp = table.Column<string>(nullable: true) // Used to prevent concurrent update conflicts
                }, // End of columns definition
                constraints: table => // Define the constraints for this table
                { // Start of constraints definition
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id); // Set the 'Id' column as the Primary Key
                }); // End of table creation

            migrationBuilder.CreateTable( // Instruction to create the user information table
                name: "AspNetUsers", // Name of the table for user accounts
                columns: table => new // Define the columns for this table
                { // Start of columns definition
                    Id = table.Column<string>(nullable: false), // Unique identifier for the user (Primary Key)
                    UserName = table.Column<string>(maxLength: 256, nullable: true), // The login name chosen by the user
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true), // Upper-cased username for searching
                    Email = table.Column<string>(maxLength: 256, nullable: true), // The user's email address
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true), // Upper-cased email for searching
                    EmailConfirmed = table.Column<bool>(nullable: false), // Flag indicating if the email has been verified
                    PasswordHash = table.Column<string>(nullable: true), // Encrypted representation of the user's password
                    SecurityStamp = table.Column<string>(nullable: true), // Random value changed whenever credentials are updated
                    ConcurrencyStamp = table.Column<string>(nullable: true), // Used for optimistic concurrency control
                    PhoneNumber = table.Column<string>(nullable: true), // The user's phone number
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false), // Flag indicating if the phone has been verified
                    TwoFactorEnabled = table.Column<bool>(nullable: false), // Flag for two-factor authentication status
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true), // Timestamp for when a user lockout expires
                    LockoutEnabled = table.Column<bool>(nullable: false), // Flag indicating if the user can be locked out
                    AccessFailedCount = table.Column<int>(nullable: false) // Count of failed login attempts
                }, // End of columns definition
                constraints: table => // Define constraints for the users table
                { // Start of constraints definition
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id); // Set 'Id' as the Primary Key
                }); // End of table creation

            migrationBuilder.CreateTable( // Instruction to create a table for role-specific claims
                name: "AspNetRoleClaims", // Name of the table linking roles to claims
                columns: table => new // Define columns
                { // Start of columns definition
                    Id = table.Column<int>(nullable: false) // Unique ID for the claim entry
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn), // Auto-incrementing ID for SQL Server
                    RoleId = table.Column<string>(nullable: false), // Reference to the Role that owns this claim
                    ClaimType = table.Column<string>(nullable: true), // The type of the claim (e.g., 'Permission')
                    ClaimValue = table.Column<string>(nullable: true) // The value associated with the claim
                }, // End of columns definition
                constraints: table => // Define constraints
                { // Start of constraints definition
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id); // Set 'Id' as the Primary Key
                    table.ForeignKey( // Define a link to the Roles table
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId", // Name of the Foreign Key constraint
                        column: x => x.RoleId, // The column in this table that is the foreign key
                        principalTable: "AspNetRoles", // The table being referenced
                        principalColumn: "Id", // The column in the referenced table
                        onDelete: ReferentialAction.Cascade); // Automatically delete claims if the role is deleted
                }); // End of table creation

            migrationBuilder.CreateTable( // Instruction to create a table for user-specific claims
                name: "AspNetUserClaims", // Name of the table linking users to claims
                columns: table => new // Define columns
                { // Start of columns definition
                    Id = table.Column<int>(nullable: false) // Unique ID for the claim entry
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn), // Auto-incrementing ID
                    UserId = table.Column<string>(nullable: false), // Reference to the User that owns this claim
                    ClaimType = table.Column<string>(nullable: true), // Type of claim
                    ClaimValue = table.Column<string>(nullable: true) // Value of claim
                }, // End of columns definition
                constraints: table => // Define constraints
                { // Start of constraints definition
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id); // Set 'Id' as the Primary Key
                    table.ForeignKey( // Link to the Users table
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId", // Name of the Foreign Key
                        column: x => x.UserId, // Foreign Key column
                        principalTable: "AspNetUsers", // Referenced table
                        principalColumn: "Id", // Referenced column
                        onDelete: ReferentialAction.Cascade); // Delete claims if the user is deleted
                }); // End of table creation

            migrationBuilder.CreateTable( // Instruction to create a table for external login providers (Google, FB, etc.)
                name: "AspNetUserLogins", // Name of the logins table
                columns: table => new // Define columns
                { // Start of columns definition
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false), // Name of the provider (e.g., 'Google')
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false), // Unique ID from the external provider
                    ProviderDisplayName = table.Column<string>(nullable: true), // User-friendly name of the provider
                    UserId = table.Column<string>(nullable: false) // Link to the internal User account
                }, // End of columns definition
                constraints: table => // Define constraints
                { // Start of constraints definition
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey }); // Composite Primary Key using Provider and Key
                    table.ForeignKey( // Link to the Users table
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId", // Foreign Key name
                        column: x => x.UserId, // FK column
                        principalTable: "AspNetUsers", // Referenced table
                        principalColumn: "Id", // Referenced column
                        onDelete: ReferentialAction.Cascade); // Delete logins if the user is deleted
                }); // End of table creation

            migrationBuilder.CreateTable( // Instruction to create a junction table for User-Role many-to-many relationship
                name: "AspNetUserRoles", // Name of the mapping table
                columns: table => new // Define columns
                { // Start of columns definition
                    UserId = table.Column<string>(nullable: false), // ID of the user
                    RoleId = table.Column<string>(nullable: false) // ID of the role assigned to that user
                }, // End of columns definition
                constraints: table => // Define constraints
                { // Start of constraints definition
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId }); // Composite Primary Key (User and Role IDs)
                    table.ForeignKey( // Link to the Roles table
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId", // FK name
                        column: x => x.RoleId, // FK column
                        principalTable: "AspNetRoles", // Referenced table
                        principalColumn: "Id", // Referenced column
                        onDelete: ReferentialAction.Cascade); // Clean up mapping if role is deleted
                    table.ForeignKey( // Link to the Users table
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId", // FK name
                        column: x => x.UserId, // FK column
                        principalTable: "AspNetUsers", // Referenced table
                        principalColumn: "Id", // Referenced column
                        onDelete: ReferentialAction.Cascade); // Clean up mapping if user is deleted
                }); // End of table creation

            migrationBuilder.CreateTable( // Instruction to create a table for security tokens (e.g., password reset, 2FA)
                name: "AspNetUserTokens", // Name of the tokens table
                columns: table => new // Define columns
                { // Start of columns definition
                    UserId = table.Column<string>(nullable: false), // ID of the user owning the token
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false), // Source of the token
                    Name = table.Column<string>(maxLength: 128, nullable: false), // Name/Type of the token
                    Value = table.Column<string>(nullable: true) // The actual token value
                }, // End of columns definition
                constraints: table => // Define constraints
                { // Start of constraints definition
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name }); // Composite Primary Key
                    table.ForeignKey( // Link to the Users table
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId", // FK name
                        column: x => x.UserId, // FK column
                        principalTable: "AspNetUsers", // Referenced table
                        principalColumn: "Id", // Referenced column
                        onDelete: ReferentialAction.Cascade); // Delete tokens if the user is deleted
                }); // End of table creation

            migrationBuilder.CreateIndex( // Create a database index for performance
                name: "IX_AspNetRoleClaims_RoleId", // Index name
                table: "AspNetRoleClaims", // Target table
                column: "RoleId"); // Column to index

            migrationBuilder.CreateIndex( // Create a unique index for role names
                name: "RoleNameIndex", // Index name
                table: "AspNetRoles", // Target table
                column: "NormalizedName", // Column to index
                unique: true, // Ensure no two roles have the same normalized name
                filter: "[NormalizedName] IS NOT NULL"); // Only apply index to non-null values

            migrationBuilder.CreateIndex( // Create an index for searching claims by user
                name: "IX_AspNetUserClaims_UserId", // Index name
                table: "AspNetUserClaims", // Target table
                column: "UserId"); // Column to index

            migrationBuilder.CreateIndex( // Create an index for searching logins by user
                name: "IX_AspNetUserLogins_UserId", // Index name
                table: "AspNetUserLogins", // Target table
                column: "UserId"); // Column to index

            migrationBuilder.CreateIndex( // Create an index for finding which users have a specific role
                name: "IX_AspNetUserRoles_RoleId", // Index name
                table: "AspNetUserRoles", // Target table
                column: "RoleId"); // Column to index

            migrationBuilder.CreateIndex( // Create an index for searching users by email
                name: "EmailIndex", // Index name
                table: "AspNetUsers", // Target table
                column: "NormalizedEmail"); // Column to index

            migrationBuilder.CreateIndex( // Create a unique index for searching users by username
                name: "UserNameIndex", // Index name
                table: "AspNetUsers", // Target table
                column: "NormalizedUserName", // Column to index
                unique: true, // Ensure no two users have the same username
                filter: "[NormalizedUserName] IS NOT NULL"); // Only apply index to non-null values
        } // End of Up method

        protected override void Down(MigrationBuilder migrationBuilder) // Method that defines how to revert the changes
        { // Start of Down method
            migrationBuilder.DropTable( // Remove the role claims table
                name: "AspNetRoleClaims"); // Table to drop

            migrationBuilder.DropTable( // Remove the user claims table
                name: "AspNetUserClaims"); // Table to drop

            migrationBuilder.DropTable( // Remove the external logins table
                name: "AspNetUserLogins"); // Table to drop

            migrationBuilder.DropTable( // Remove the user roles mapping table
                name: "AspNetUserRoles"); // Table to drop

            migrationBuilder.DropTable( // Remove the security tokens table
                name: "AspNetUserTokens"); // Table to drop

            migrationBuilder.DropTable( // Remove the roles table
                name: "AspNetRoles"); // Table to drop

            migrationBuilder.DropTable( // Remove the users table
                name: "AspNetUsers"); // Table to drop
        } // End of Down method
    } // End of class
} // End of namespace