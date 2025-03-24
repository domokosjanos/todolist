CREATE DATABASE teendolista CHARACTER SET utf8mb4 COLLATE utf8mb4_hungarian_ci;

USE teendolista;
--Táblák
CREATE TABLE Felhasznalo ( 
id INT AUTO_INCREMENT NOT NULL,
Vnev VARCHAR(40) NOT NULL,
Knev VARCHAR(40) NOT NULL,
Hnev VARCHAR(40) NULL, 
Szul_ido DATE NOT NULL, 
Fnev VARCHAR(30) NOT NULL, 
Jelszo TEXT NOT NULL, 
Email VARCHAR(255) NOT NULL,
Telszam VARCHAR(15) NOT NULL, 
PRIMARY KEY(id),
UNIQUE KEY(Fnev)
)
;

CREATE TABLE Csoportok (
id INT AUTO_INCREMENT NOT NULL,
Nev VARCHAR(40) NOT NULL,
Letrehozo INT NOT NULL,
Letszam INT NULL,
PRIMARY KEY(id)
 )
;

CREATE TABLE Feladatok (
id INT AUTO_INCREMENT NOT NULL,
Leiras TEXT NOT NULL,
Hatarido DATE NULL,
Allapot BOOLEAN NOT NULL,
Letrejotte DATE NULL,
FHO_id INT NOT NULL,
PRIMARY KEY(id, FHO_id), FOREIGN KEY(FHO_id) REFERENCES Felhasznalo(id) ON DELETE CASCADE
)
;

CREATE TABLE Tagsagok (
FHO_id INT NOT NULL,
CSPT_id INT NOT NULL,
Jogosultsag VARCHAR(15) NOT NULL,
PRIMARY KEY(FHO_id, CSPT_id),
FOREIGN KEY(FHO_id) REFERENCES Felhasznalo(id) ON DELETE CASCADE,
FOREIGN KEY(CSPT_id) REFERENCES Csoportok(id) ON DELETE CASCADE
)
;

CREATE TABLE Felelosok (
CSPT_id INT NOT NULL,
FAT_id INT NOT NULL,
PRIMARY KEY(CSPT_id, FAT_id),
FOREIGN KEY(CSPT_id) REFERENCES Csoportok(id) ON DELETE CASCADE,
FOREIGN KEY(FAT_id) REFERENCES Feladatok(id) ON DELETE CASCADE
)
;
--Példasorok
INSERT INTO Felhasznalo (Vnev, Knev, Hnev, Szul_ido, Fnev, Jelszo, Email, Telszam) VALUES
('Anna', 'Kovács', 'Károly', '1990-05-20', 'anna.kovacs', 'jelszo123', 'anna.k@example.com', '06123456789'),
('Béla', 'Nagy', 'Nagy', '1985-03-15', 'bela.nagy', 'jelszo456', 'bela.n@example.com', '06123456788'),
('Csaba', 'Tóth', 'Tamás', '1992-08-30', 'csaba.toth', 'jelszo789', 'csaba.t@example.com', '06123456787'),
('Dóra', 'Farkas', 'Ferenc', '1988-11-05', 'dora.farkas', 'jelszo101', 'dora.f@example.com', '06123456786')
;

INSERT INTO Csoportok (Nev, Letrehozo,Letszam) VALUES
('Fejlesztők', 1,5),
('Tesztelők', 2,1),
('Üzemeltetők', 3,0),
('Adminok', 4,7)
;

INSERT INTO Feladatok (Leiras, Hatarido, Allapot, FHO_id) VALUES
('Weboldal fejlesztése', '2024-12-31', TRUE, 1),
('Funkcionalitás tesztelése', '2024-11-30', FALSE, 2),
('Szerver karbantartás', '2024-10-15', TRUE, 3),
('Dokumentáció frissítése', '2024-09-01', FALSE, 4),
('Valami valami valami valami valami valami valami ', '2024-12-15', TRUE, 4),
('Hálózat karbantartása valahol valahol ', '2025-06-01', TRUE, 4)
;

INSERT INTO Tagsagok (FHO_id, CSPT_id, Jogosultsag) VALUES
(1, 1, 'admin'),
(2, 3, 'admin'),
(3, 2, 'admin'),
(4, 4, 'admin'),
(1, 4, 'member'),
(2, 2, 'member'),
(3, 3, 'member'),
(4, 3, 'member')
;

INSERT INTO Felelosok (CSPT_id, FAT_id) VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(4, 5),
(4, 6)
;
--DROP DATABASE