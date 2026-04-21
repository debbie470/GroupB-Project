using System; // Import the base System namespace for core types like DateTime
using Microsoft.EntityFrameworkCore.Migrations; // Import EF Core Migrations namespace for schema modification tools

#nullable disable // Disable nullable reference type checking for this migration file

namespace ECommercePlatform.Data.Migrations // Define the namespace for the database migrations
{ // Start of the namespace block
    /// <inheritdoc /> // Inherit XML documentation from the base Migration class
    public partial class AddingAllModelsAsControllers : Migration // Define a partial class that inherits from Migration
    { // Start of the class block
        /// <inheritdoc /> // Inherit documentation for the Up method
        protected override void Up(MigrationBuilder migrationBuilder) // Method used to apply schema changes to the database
        { // Start of the Up method
            migrationBuilder.CreateTable( // Command to create the 'Basket' table
                name: "Basket", // Name of the table in the database
                columns: table => new // Define the columns for the table
                { // Start of columns definition
                    BasketId = table.Column<int>(type: "int", nullable: false) // Primary key column of type integer
                        .Annotation("SqlServer:Identity", "1, 1"), // Configure as an auto-incrementing identity column (start at 1, increment by 1)
                    Status = table.Column<bool>(type: "bit", nullable: false), // Boolean column for basket status
                    BasketCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false), // Timestamp for when the basket was created
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false) // String column to store the owner's User ID
                }, // End of columns definition
                constraints: table => // Define table-level constraints
                { // Start of constraints definition
                    table.PrimaryKey("PK_Basket", x => x.BasketId); // Set 'BasketId' as the Primary Key
                }); // End of table creation

            migrationBuilder.CreateTable( // Command to create the 'LoyaltyRewards' table
                name: "LoyaltyRewards", // Table name
                columns: table => new // Define columns
                { // Start of columns definition
                    LoyaltyRewardsId = table.Column<int>(type: "int", nullable: false) // Primary Key column
                        .Annotation("SqlServer:Identity", "1, 1"), // Auto-incrementing identity
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false), // ID of the user owning the rewards
                    PointsBalance = table.Column<int>(type: "int", nullable: false), // Integer column for reward points
                    TierLevel = table.Column<string>(type: "nvarchar(max)", nullable: false), // String for membership tier (e.g., Gold)
                    History = table.Column<string>(type: "nvarchar(max)", nullable: false) // String for reward transaction history
                }, // End of columns definition
                constraints: table => // Define constraints
                { // Start of constraints definition
                    table.PrimaryKey("PK_LoyaltyRewards", x => x.LoyaltyRewardsId); // Set 'LoyaltyRewardsId' as Primary Key
                }); // End of table creation

            migrationBuilder.CreateTable( // Command to create the 'Orders' table
                name: "Orders", // Table name
                columns: table => new // Define columns
                { // Start of columns definition
                    OrdersId = table.Column<int>(type: "int", nullable: false) // Primary Key column
                        .Annotation("SqlServer:Identity", "1, 1"), // Auto-incrementing identity
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false), // ID of the user who placed the order
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false), // Decimal column for order cost (18 digits, 2 decimal places)
                    Delivery = table.Column<bool>(type: "bit", nullable: false), // Flag indicating if delivery is selected
                    Collection = table.Column<bool>(type: "bit", nullable: false), // Flag indicating if collection is selected
                    DeliveryType = table.Column<string>(type: "nvarchar(max)", nullable: true), // Nullable string for delivery method details
                    OrderTrackingStatus = table.Column<string>(type: "nvarchar(max)", nullable: false), // String for current status (e.g., Pending)
                    CollectionDate = table.Column<DateOnly>(type: "date", nullable: true), // Nullable date column for collection
                    OrderDate = table.Column<DateOnly>(type: "date", nullable: false) // Date column for when the order was placed
                }, // End of columns definition
                constraints: table => // Define constraints
                { // Start of constraints definition
                    table.PrimaryKey("PK_Orders", x => x.OrdersId); // Set 'OrdersId' as Primary Key
                }); // End of table creation

            migrationBuilder.CreateTable( // Command to create the 'Suppliers' table
                name: "Suppliers", // Table name
                columns: table => new // Define columns
                { // Start of columns definition
                    SuppliersId = table.Column<int>(type: "int", nullable: false) // Primary Key column
                        .Annotation("SqlServer:Identity", "1, 1"), // Auto-incrementing identity
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false), // ID for linking the supplier to a system user
                    SupplierName = table.Column<string>(type: "nvarchar(max)", nullable: false), // Name of the supplier company
                    SupplierEmail = table.Column<string>(type: "nvarchar(max)", nullable: false), // Contact email for the supplier
                    SupplierInformation = table.Column<string>(type: "nvarchar(max)", nullable: false) // Description or details about the supplier
                }, // End of columns definition
                constraints: table => // Define constraints
                { // Start of constraints definition
                    table.PrimaryKey("PK_Suppliers", x => x.SuppliersId); // Set 'SuppliersId' as Primary Key
                }); // End of table creation

            migrationBuilder.CreateTable( // Command to create the 'DeliveryInfo' table
                name: "DeliveryInfo", // Table name
                columns: table => new // Define columns
                { // Start of columns definition
                    DeliveryInfoId = table.Column<int>(type: "int", nullable: false) // Primary Key
                        .Annotation("SqlServer:Identity", "1, 1"), // Auto-incrementing
                    OrdersId = table.Column<int>(type: "int", nullable: false), // Foreign Key linking to the Orders table
                    DeliveryType = table.Column<string>(type: "nvarchar(max)", nullable: false), // Specific delivery method string
                    ScheduledDateTime = table.Column<DateTime>(type: "datetime2", nullable: false), // Date and time for planned delivery
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false) // Current status of the delivery
                }, // End of columns definition
                constraints: table => // Define constraints
                { // Start of constraints definition
                    table.PrimaryKey("PK_DeliveryInfo", x => x.DeliveryInfoId); // Set 'DeliveryInfoId' as Primary Key
                    table.ForeignKey( // Define a Foreign Key relationship
                        name: "FK_DeliveryInfo_Orders_OrdersId", // Unique name for the foreign key
                        column: x => x.OrdersId, // Local column that holds the foreign key
                        principalTable: "Orders", // Referenced (parent) table
                        principalColumn: "OrdersId", // Referenced column in the parent table
                        onDelete: ReferentialAction.Cascade); // If an Order is deleted, delete its DeliveryInfo automatically
                }); // End of table creation

            migrationBuilder.CreateTable( // Command to create the 'Products' table
                name: "Products", // Table name
                columns: table => new // Define columns
                { // Start of columns definition
                    ProductsId = table.Column<int>(type: "int", nullable: false) // Primary Key
                        .Annotation("SqlServer:Identity", "1, 1"), // Auto-incrementing
                    SuppliersId = table.Column<int>(type: "int", nullable: false), // Foreign Key linking to the Suppliers table
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false), // Name of the product
                    Stock = table.Column<int>(type: "int", nullable: false), // Integer for available quantity
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false), // Price with precision
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true) // Nullable string for product image location
                }, // End of columns definition
                constraints: table => // Define constraints
                { // Start of constraints definition
                    table.PrimaryKey("PK_Products", x => x.ProductsId); // Set 'ProductsId' as Primary Key
                    table.ForeignKey( // Define a Foreign Key relationship
                        name: "FK_Products_Suppliers_SuppliersId", // FK name
                        column: x => x.SuppliersId, // FK column
                        principalTable: "Suppliers", // Referenced table
                        principalColumn: "SuppliersId", // Referenced column
                        onDelete: ReferentialAction.Cascade); // Delete products if their supplier is deleted
                }); // End of table creation

            migrationBuilder.CreateTable( // Command to create the 'BasketProducts' join table (many-to-many link)
                name: "BasketProducts", // Table name
                columns: table => new // Define columns
                { // Start of columns definition
                    BasketProductsId = table.Column<int>(type: "int", nullable: false) // Primary Key
                        .Annotation("SqlServer:Identity", "1, 1"), // Auto-incrementing
                    BasketId = table.Column<int>(type: "int", nullable: false), // Foreign Key linking to Basket
                    ProductsId = table.Column<int>(type: "int", nullable: false), // Foreign Key linking to Products
                    Quantity = table.Column<int>(type: "int", nullable: false) // Number of items in this specific basket entry
                }, // End of columns definition
                constraints: table => // Define constraints
                { // Start of constraints definition
                    table.PrimaryKey("PK_BasketProducts", x => x.BasketProductsId); // Set 'BasketProductsId' as Primary Key
                    table.ForeignKey( // Link to the Basket table
                        name: "FK_BasketProducts_Basket_BasketId", // FK name
                        column: x => x.BasketId, // FK column
                        principalTable: "Basket", // Parent table
                        principalColumn: "BasketId", // Parent column
                        onDelete: ReferentialAction.Cascade); // Cleanup if Basket is removed
                    table.ForeignKey( // Link to the Products table
                        name: "FK_BasketProducts_Products_ProductsId", // FK name
                        column: x => x.ProductsId, // FK column
                        principalTable: "Products", // Parent table
                        principalColumn: "ProductsId", // Parent column
                        onDelete: ReferentialAction.Cascade); // Cleanup if Product is removed
                }); // End of table creation

            migrationBuilder.CreateTable( // Command to create the 'OrderProducts' join table
                name: "OrderProducts", // Table name
                columns: table => new // Define columns
                { // Start of columns definition
                    OrderProductsId = table.Column<int>(type: "int", nullable: false) // Primary Key
                        .Annotation("SqlServer:Identity", "1, 1"), // Auto-incrementing
                    ProductsId = table.Column<int>(type: "int", nullable: false), // FK to Products
                    OrdersId = table.Column<int>(type: "int", nullable: false), // FK to Orders
                    Quantity = table.Column<int>(type: "int", nullable: false) // Quantity ordered
                }, // End of columns definition
                constraints: table => // Define constraints
                { // Start of constraints definition
                    table.PrimaryKey("PK_OrderProducts", x => x.OrderProductsId); // Set 'OrderProductsId' as Primary Key
                    table.ForeignKey( // Link to the Orders table
                        name: "FK_OrderProducts_Orders_OrdersId", // FK name
                        column: x => x.OrdersId, // FK column
                        principalTable: "Orders", // Parent table
                        principalColumn: "OrdersId", // Parent column
                        onDelete: ReferentialAction.Cascade); // Cleanup if Order is deleted
                    table.ForeignKey( // Link to the Products table
                        name: "FK_OrderProducts_Products_ProductsId", // FK name
                        column: x => x.ProductsId, // FK column
                        principalTable: "Products", // Parent table
                        principalColumn: "ProductsId", // Parent column
                        onDelete: ReferentialAction.Cascade); // Cleanup if Product is deleted
                }); // End of table creation

            migrationBuilder.CreateTable( // Command to create the 'ProductTraceability' table
                name: "ProductTraceability", // Table name
                columns: table => new // Define columns
                { // Start of columns definition
                    ProductTraceabilityId = table.Column<int>(type: "int", nullable: false) // Primary Key
                        .Annotation("SqlServer:Identity", "1, 1"), // Auto-incrementing
                    ProductsId = table.Column<int>(type: "int", nullable: false), // FK link to the specific Product
                    Origin = table.Column<string>(type: "nvarchar(max)", nullable: false), // Where the product came from
                    BatchNumber = table.Column<string>(type: "nvarchar(max)", nullable: false), // Manufacturing/Logistics batch identifier
                    HarvestDate = table.Column<DateOnly>(type: "date", nullable: false), // When the product was harvested
                    Certifications = table.Column<string>(type: "nvarchar(max)", nullable: false) // Associated safety/quality certificates
                }, // End of columns definition
                constraints: table => // Define constraints
                { // Start of constraints definition
                    table.PrimaryKey("PK_ProductTraceability", x => x.ProductTraceabilityId); // Set 'ProductTraceabilityId' as Primary Key
                    table.ForeignKey( // Link to the Products table
                        name: "FK_ProductTraceability_Products_ProductsId", // FK name
                        column: x => x.ProductsId, // FK column
                        principalTable: "Products", // Parent table
                        principalColumn: "ProductsId", // Parent column
                        onDelete: ReferentialAction.Cascade); // Cleanup if Product is deleted
                }); // End of table creation

            migrationBuilder.CreateIndex( // Create a performance index for BasketId on the join table
                name: "IX_BasketProducts_BasketId", // Index name
                table: "BasketProducts", // Target table
                column: "BasketId"); // Target column

            migrationBuilder.CreateIndex( // Create a performance index for ProductsId on the join table
                name: "IX_BasketProducts_ProductsId", // Index name
                table: "BasketProducts", // Target table
                column: "ProductsId"); // Target column

            migrationBuilder.CreateIndex( // Create a performance index for OrdersId on DeliveryInfo
                name: "IX_DeliveryInfo_OrdersId", // Index name
                table: "DeliveryInfo", // Target table
                column: "OrdersId"); // Target column

            migrationBuilder.CreateIndex( // Create a performance index for OrdersId on the order items table
                name: "IX_OrderProducts_OrdersId", // Index name
                table: "OrderProducts", // Target table
                column: "OrdersId"); // Target column

            migrationBuilder.CreateIndex( // Create a performance index for ProductsId on the order items table
                name: "IX_OrderProducts_ProductsId", // Index name
                table: "OrderProducts", // Target table
                column: "ProductsId"); // Target column

            migrationBuilder.CreateIndex( // Create a performance index for SuppliersId on the Products table
                name: "IX_Products_SuppliersId", // Index name
                table: "Products", // Target table
                column: "SuppliersId"); // Target column

            migrationBuilder.CreateIndex( // Create a performance index for ProductsId on the traceability table
                name: "IX_ProductTraceability_ProductsId", // Index name
                table: "ProductTraceability", // Target table
                column: "ProductsId"); // Target column
        } // End of Up method

        /// <inheritdoc /> // Inherit documentation for the Down method
        protected override void Down(MigrationBuilder migrationBuilder) // Method used to rollback/revert the changes if needed
        { // Start of the Down method
            migrationBuilder.DropTable( // Remove the 'BasketProducts' table
                name: "BasketProducts"); // Table to drop

            migrationBuilder.DropTable( // Remove the 'DeliveryInfo' table
                name: "DeliveryInfo"); // Table to drop

            migrationBuilder.DropTable( // Remove the 'LoyaltyRewards' table
                name: "LoyaltyRewards"); // Table to drop

            migrationBuilder.DropTable( // Remove the 'OrderProducts' table
                name: "OrderProducts"); // Table to drop

            migrationBuilder.DropTable( // Remove the 'ProductTraceability' table
                name: "ProductTraceability"); // Table to drop

            migrationBuilder.DropTable( // Remove the 'Basket' table
                name: "Basket"); // Table to drop

            migrationBuilder.DropTable( // Remove the 'Orders' table
                name: "Orders"); // Table to drop

            migrationBuilder.DropTable( // Remove the 'Products' table
                name: "Products"); // Table to drop

            migrationBuilder.DropTable( // Remove the 'Suppliers' table
                name: "Suppliers"); // Table to drop
        } // End of the Down method
    } // End of class
} // End of namespace