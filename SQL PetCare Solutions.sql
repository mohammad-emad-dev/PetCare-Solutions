IF DB_ID(N'PetCareSolutions') IS NULL
BEGIN
    CREATE DATABASE PetCareSolutions;
END
GO

USE PetCareSolutions;
GO

IF OBJECT_ID(N'dbo.owners', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.owners (
        id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_owners PRIMARY KEY,
        firstName NVARCHAR(50) NOT NULL,
        lastName NVARCHAR(50) NOT NULL,
        phoneNumber NVARCHAR(20) NULL,
        email NVARCHAR(100) NULL CONSTRAINT UQ_owners_email UNIQUE,
        street NVARCHAR(255) NULL,
        city NVARCHAR(100) NULL
    );
END
GO

IF OBJECT_ID(N'dbo.users', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.users (
        id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_users PRIMARY KEY,
        username NVARCHAR(50) NOT NULL CONSTRAINT UQ_users_username UNIQUE,
        passwordHash NVARCHAR(255) NOT NULL,
        role NVARCHAR(50) NOT NULL,
        dateCreated DATETIME NOT NULL CONSTRAINT DF_users_dateCreated DEFAULT GETDATE()
    );
END
GO

IF OBJECT_ID(N'dbo.veterinarians', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.veterinarians (
        id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_veterinarians PRIMARY KEY,
        name NVARCHAR(100) NOT NULL,
        specialty NVARCHAR(100) NULL,
        contactInfo NVARCHAR(255) NULL,
        userId INT NOT NULL CONSTRAINT UQ_veterinarians_userId UNIQUE,
        CONSTRAINT FK_veterinarians_users FOREIGN KEY (userId) REFERENCES dbo.users(id)
    );
END
GO

IF OBJECT_ID(N'dbo.pets', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.pets (
        id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_pets PRIMARY KEY,
        name NVARCHAR(100) NOT NULL,
        species NVARCHAR(50) NULL,
        breed NVARCHAR(50) NULL,
        Gender NVARCHAR(10) NULL,
        dateOfBirth DATE NULL,
        medicalNotes NVARCHAR(MAX) NULL,
        ownerId INT NOT NULL,
        CONSTRAINT FK_pets_owners FOREIGN KEY (ownerId) REFERENCES dbo.owners(id)
    );
END
GO

IF OBJECT_ID(N'dbo.appointments', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.appointments (
        id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_appointments PRIMARY KEY,
        appointmentDate DATE NOT NULL,
        appointmentTime TIME NOT NULL,
        typeOfService NVARCHAR(100) NULL,
        status NVARCHAR(20) NOT NULL CONSTRAINT DF_appointments_status DEFAULT N'Scheduled',
        petId INT NOT NULL,
        vetId INT NOT NULL,
        CONSTRAINT FK_appointments_pets FOREIGN KEY (petId) REFERENCES dbo.pets(id),
        CONSTRAINT FK_appointments_veterinarians FOREIGN KEY (vetId) REFERENCES dbo.veterinarians(id)
    );
END
GO

IF OBJECT_ID(N'dbo.medical_records', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.medical_records (
        petId INT NOT NULL,
        id INT IDENTITY(1,1) NOT NULL,
        diagnosis NVARCHAR(MAX) NULL,
        prescriptions NVARCHAR(MAX) NULL,
        vaccinationsGiven NVARCHAR(MAX) NULL,
        followupInstructions NVARCHAR(MAX) NULL,
        vetId INT NOT NULL,
        CONSTRAINT PK_medical_records PRIMARY KEY (petId, id),
        CONSTRAINT FK_medical_records_pets FOREIGN KEY (petId) REFERENCES dbo.pets(id),
        CONSTRAINT FK_medical_records_veterinarians FOREIGN KEY (vetId) REFERENCES dbo.veterinarians(id)
    );
END
GO

IF OBJECT_ID(N'dbo.BookingRequests', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.BookingRequests (
        RequestID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_BookingRequests PRIMARY KEY,
        OwnerID INT NOT NULL,
        PetID INT NOT NULL,
        RequestedDate DATE NOT NULL,
        RequestedTime TIME NOT NULL,
        Status NVARCHAR(20) NOT NULL CONSTRAINT DF_BookingRequests_Status DEFAULT N'Pending',
        CONSTRAINT FK_BookingRequests_owners FOREIGN KEY (OwnerID) REFERENCES dbo.owners(id),
        CONSTRAINT FK_BookingRequests_pets FOREIGN KEY (PetID) REFERENCES dbo.pets(id)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.users WHERE username = N'ADMIN1')
    INSERT INTO dbo.users (username, passwordHash, role) VALUES (N'ADMIN1', N'1111', N'Administrator');

IF NOT EXISTS (SELECT 1 FROM dbo.users WHERE username = N'ADMIN')
    INSERT INTO dbo.users (username, passwordHash, role) VALUES (N'ADMIN', N'ADMIN', N'Administrator');

IF NOT EXISTS (SELECT 1 FROM dbo.users WHERE username = N'dr.ahmad')
    INSERT INTO dbo.users (username, passwordHash, role) VALUES (N'dr.ahmad', N'password1', N'Veterinarian');

IF NOT EXISTS (SELECT 1 FROM dbo.users WHERE username = N'dr.fatima')
    INSERT INTO dbo.users (username, passwordHash, role) VALUES (N'dr.fatima', N'password2', N'Veterinarian');

IF NOT EXISTS (SELECT 1 FROM dbo.users WHERE username = N'sara.reception')
    INSERT INTO dbo.users (username, passwordHash, role) VALUES (N'sara.reception', N'password3', N'Receptionist');
GO

IF NOT EXISTS (SELECT 1 FROM dbo.owners WHERE email = N'sami.k@example.com')
    INSERT INTO dbo.owners (firstName, lastName, phoneNumber, email, street, city)
    VALUES (N'Sami', N'Khaled', N'0791234567', N'sami.k@example.com', N'123 Al-Rainbow St', N'Amman');

IF NOT EXISTS (SELECT 1 FROM dbo.owners WHERE email = N'nadia.s@example.com')
    INSERT INTO dbo.owners (firstName, lastName, phoneNumber, email, street, city)
    VALUES (N'Nadia', N'Saleh', N'0788765432', N'nadia.s@example.com', N'45 Queen Rania St', N'Irbid');

IF NOT EXISTS (SELECT 1 FROM dbo.owners WHERE email = N'omar.z@example.com')
    INSERT INTO dbo.owners (firstName, lastName, phoneNumber, email, street, city)
    VALUES (N'Omar', N'Zaid', N'0777112233', N'omar.z@example.com', N'78 Gardens St', N'Amman');
GO

IF NOT EXISTS (SELECT 1 FROM dbo.veterinarians WHERE name = N'Dr. Ahmad Yaseen')
    INSERT INTO dbo.veterinarians (name, specialty, contactInfo, userId)
    SELECT N'Dr. Ahmad Yaseen', N'General Practice and Surgery', N'ayaseen@petcare.com', id
    FROM dbo.users
    WHERE username = N'dr.ahmad';

IF NOT EXISTS (SELECT 1 FROM dbo.veterinarians WHERE name = N'Dr. Fatima Ali')
    INSERT INTO dbo.veterinarians (name, specialty, contactInfo, userId)
    SELECT N'Dr. Fatima Ali', N'Dermatology', N'fali@petcare.com', id
    FROM dbo.users
    WHERE username = N'dr.fatima';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.pets WHERE name = N'Mishmish')
    INSERT INTO dbo.pets (name, species, breed, Gender, dateOfBirth, medicalNotes, ownerId)
    SELECT N'Mishmish', N'Cat', N'Shirazi', N'Male', '2022-04-10', N'Allergic to dust.', id
    FROM dbo.owners
    WHERE email = N'sami.k@example.com';

IF NOT EXISTS (SELECT 1 FROM dbo.pets WHERE name = N'Leo')
    INSERT INTO dbo.pets (name, species, breed, Gender, dateOfBirth, medicalNotes, ownerId)
    SELECT N'Leo', N'Dog', N'German Shepherd', N'Male', '2021-08-20', N'Slightly sensitive stomach.', id
    FROM dbo.owners
    WHERE email = N'nadia.s@example.com';

IF NOT EXISTS (SELECT 1 FROM dbo.pets WHERE name = N'Kiwi')
    INSERT INTO dbo.pets (name, species, breed, Gender, dateOfBirth, medicalNotes, ownerId)
    SELECT N'Kiwi', N'Bird', N'Cockatiel', N'Female', '2023-01-15', N'Very active and healthy.', id
    FROM dbo.owners
    WHERE email = N'omar.z@example.com';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.appointments)
BEGIN
    INSERT INTO dbo.appointments (appointmentDate, appointmentTime, typeOfService, status, petId, vetId)
    SELECT '2026-06-02', '10:00:00', N'Annual Checkup', N'Scheduled', p.id, v.id
    FROM dbo.pets p
    CROSS JOIN dbo.veterinarians v
    WHERE p.name = N'Mishmish' AND v.name = N'Dr. Ahmad Yaseen';

    INSERT INTO dbo.appointments (appointmentDate, appointmentTime, typeOfService, status, petId, vetId)
    SELECT '2026-06-03', '11:30:00', N'Vaccination', N'Scheduled', p.id, v.id
    FROM dbo.pets p
    CROSS JOIN dbo.veterinarians v
    WHERE p.name = N'Leo' AND v.name = N'Dr. Fatima Ali';
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.medical_records)
BEGIN
    INSERT INTO dbo.medical_records (petId, diagnosis, prescriptions, vaccinationsGiven, followupInstructions, vetId)
    SELECT p.id, N'Minor ear infection', N'Otic-Plus ear drops twice daily for 7 days.', N'None', N'Re-check in one week if symptoms persist.', v.id
    FROM dbo.pets p
    CROSS JOIN dbo.veterinarians v
    WHERE p.name = N'Mishmish' AND v.name = N'Dr. Ahmad Yaseen';

    INSERT INTO dbo.medical_records (petId, diagnosis, prescriptions, vaccinationsGiven, followupInstructions, vetId)
    SELECT p.id, N'Routine vaccination update', N'None', N'Rabies booster', N'Monitor for lethargy or swelling.', v.id
    FROM dbo.pets p
    CROSS JOIN dbo.veterinarians v
    WHERE p.name = N'Leo' AND v.name = N'Dr. Fatima Ali';
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.BookingRequests)
BEGIN
    INSERT INTO dbo.BookingRequests (OwnerID, PetID, RequestedDate, RequestedTime, Status)
    SELECT o.id, p.id, '2026-06-05', '14:00:00', N'Pending'
    FROM dbo.owners o
    JOIN dbo.pets p ON p.ownerId = o.id
    WHERE o.email = N'omar.z@example.com' AND p.name = N'Kiwi';
END
GO

SELECT username, role, dateCreated
FROM dbo.users
ORDER BY username;

SELECT p.name AS PetName, p.species, o.firstName AS OwnerFirstName, o.lastName AS OwnerLastName
FROM dbo.pets AS p
JOIN dbo.owners AS o ON p.ownerId = o.id
ORDER BY p.name;

SELECT a.appointmentDate, a.appointmentTime, p.name AS PetName, v.name AS VeterinarianName, a.status
FROM dbo.appointments AS a
JOIN dbo.pets AS p ON a.petId = p.id
JOIN dbo.veterinarians AS v ON a.vetId = v.id
ORDER BY a.appointmentDate, a.appointmentTime;
