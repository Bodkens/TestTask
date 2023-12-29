CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;

CREATE TABLE "Projects" (
    "ID" INTEGER NOT NULL CONSTRAINT "PK_Projects" PRIMARY KEY AUTOINCREMENT,
    "ConstrucionID" INTEGER NOT NULL,
    "OrganizationID" INTEGER NOT NULL,
    "Location" TEXT NOT NULL,
    "Price" decimal(19,8) NOT NULL
);

CREATE TABLE "Constructions" (
    "ID" INTEGER NOT NULL CONSTRAINT "PK_Constructions" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "StartDate" TEXT NOT NULL,
    "FinishDate" TEXT NOT NULL,
    "ExpectedPrice" decimal(19,8) NOT NULL,
    "ProjectID" INTEGER NULL,
    CONSTRAINT "FK_Constructions_Projects_ProjectID" FOREIGN KEY ("ProjectID") REFERENCES "Projects" ("ID")
);

CREATE TABLE "Organizations" (
    "ID" INTEGER NOT NULL CONSTRAINT "PK_Organizations" PRIMARY KEY AUTOINCREMENT,
    "Type" INTEGER NOT NULL,
    "CountryCode" TEXT NOT NULL,
    "VATNumber" TEXT NOT NULL,
    "Name" TEXT NOT NULL,
    "ProjectID" INTEGER NULL,
    CONSTRAINT "FK_Organizations_Projects_ProjectID" FOREIGN KEY ("ProjectID") REFERENCES "Projects" ("ID")
);

CREATE INDEX "IX_Constructions_ProjectID" ON "Constructions" ("ProjectID");

CREATE INDEX "IX_Organizations_ProjectID" ON "Organizations" ("ProjectID");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20231227015139_Initial', '8.0.0');

COMMIT;

