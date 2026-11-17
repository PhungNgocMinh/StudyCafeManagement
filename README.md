# StudyCafeManagement

## Setup

Open `StudyCafeManagement.sln` in Visual Studio and restore the NuGet package
listed in `StudyCafeManagement\packages.config`. To create a fresh database,
run `StudyCafeManagement\Create_StudyCafeManagement_Database.sql` in SQL Server
Management Studio. It creates `QuanLyCaPheHocTap` and inserts the initial data;
it stops without changing anything if that database already exists.

`StudyCafeManagement\Update_Role_Constraint.sql` is only for updating an older
database that still has the role-name constraint. `Seed_Ban.sql` at the
repository root is an optional script for synchronizing the standard tables
in an existing database; it is not needed for a fresh setup.

The application starts at `FrmLogin`. After a successful login, every role opens
`FrmMain`, which displays the active username and role and provides navigation
to the forms available to that role:

- **Admin:** all catalog and account management forms, ordering, and reports.
- **Thu ngân:** ordering/payment workflow, revenue and best-seller reports, and
  password changes.
- **Pha chế:** the shared order workflow and password changes. This project has
  no separate kitchen screen, so `FrmOrder` is the available operational form.

The two reports are available from **Nghiệp vụ** and show paid-invoice revenue
and the top five products for a selected date range. Payment is opened from an
active order in `FrmOrder`.