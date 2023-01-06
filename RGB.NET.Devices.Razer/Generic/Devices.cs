using System;
using System.Collections.Generic;

namespace RGB.NET.Devices.Razer;

internal class Devices
{
    public static readonly List<(Guid guid, string model)> KEYBOARDS = new()
                                                                       {
                                                                           (new Guid("2EA1BB63-CA28-428D-9F06-196B88330BBB"), "Blackwidow Chroma"),
                                                                           (new Guid("ED1C1B82-BFBE-418F-B49D-D03F05B149DF"), "Razer Blackwidow Chroma Tournament Edition"),
                                                                           (new Guid("18C5AD9B-4326-4828-92C4-2669A66D2283"), "Razer Deathstalker "),
                                                                           (new Guid("872AB2A9-7959-4478-9FED-15F6186E72E4"), "Overwatch Keyboard"),
                                                                           (new Guid("5AF60076-ADE9-43D4-B574-52599293B554"), "Razer Blackwidow X Chroma"),
                                                                           (new Guid("2D84DD51-3290-4AAC-9A89-D8AFDE38B57C"), "Razer Blackwidow X TE Chroma"),
                                                                           (new Guid("803378C1-CC48-4970-8539-D828CC1D420A"), "Razer Omata Chroma"),
                                                                           (new Guid("C83BDFE8-E7FC-40E0-99DB-872E23F19891"), "Razer Blade Stealth"),
                                                                           (new Guid("F2BEDFAF-A0FE-4651-9D41-B6CE603A3DDD"), "Razer Blade"),
                                                                           (new Guid("A73AC338-F0E5-4BF7-91AE-DD1F7E1737A5"), "Razer Blade Pro"),
                                                                           (new Guid("608E743F-B402-44BD-A7A6-7AA9F574ECF4"), "Razer Blackwidow Chroma v2"),
                                                                           (new Guid("F85E7473-8F03-45B6-A16E-CE26CB8D2441"), "Razer Huntsman"),
                                                                           (new Guid("16BB5ABD-C1CD-4CB3-BDF7-62438748BD98"), "Razer Blackwidow Elite")
                                                                       };

    public static readonly List<(Guid guid, string model)> MICE = new()
                                                                  {
                                                                      (new Guid("7EC00450-E0EE-4289-89D5-0D879C19061A"), "Razer Mamba Chroma Tournament Edition"),
                                                                      (new Guid("AEC50D91-B1F1-452F-8E16-7B73F376FDF3"), "Razer Deathadder Chroma "),
                                                                      (new Guid("FF8A5929-4512-4257-8D59-C647BF9935D0"), "Razer Diamondback"),
                                                                      (new Guid("D527CBDC-EB0A-483A-9E89-66D50463EC6C"), "Razer Mamba"),
                                                                      (new Guid("D714C50B-7158-4368-B99C-601ACB985E98"), "Razer Naga Epic"),
                                                                      (new Guid("F1876328-6CA4-46AE-BE04-BE812B414433"), "Razer Naga"),
                                                                      (new Guid("52C15681-4ECE-4DD9-8A52-A1418459EB34"), "Razer Orochi Chroma"),
                                                                      (new Guid("195D70F5-F285-4CFF-99F2-B8C0E9658DB4"), "Razer Naga Hex Chroma"),
                                                                      (new Guid("77834867-3237-4A9F-AD77-4A46C4183003"), "Razer DeathAdder Elite Chroma")
                                                                  };

    public static readonly List<(Guid guid, string model)> HEADSETS = new()
                                                                      {
                                                                          (new Guid("DF3164D7-5408-4A0E-8A7F-A7412F26BEBF"), "Razer ManO'War"),
                                                                          (new Guid("CD1E09A5-D5E6-4A6C-A93B-E6D9BF1D2092"), "Razer Kraken 7.1 Chroma"),
                                                                          (new Guid("7FB8A36E-9E74-4BB3-8C86-CAC7F7891EBD"), "Razer Kraken 7.1 Chroma Refresh"),
                                                                          (new Guid("FB357780-4617-43A7-960F-D1190ED54806"), "Razer Kraken Kitty")
                                                                      };

    public static readonly List<(Guid guid, string model)> MOUSEMATS = new()
                                                                       {
                                                                           (new Guid("80F95A94-73D2-48CA-AE9A-0986789A9AF2"), "Razer Firefly")
                                                                       };

    public static readonly List<(Guid guid, string model)> KEYPADS = new()
                                                                     {
                                                                         (new Guid("9D24B0AB-0162-466C-9640-7A924AA4D9FD"), "Razer Orbweaver"),
                                                                         (new Guid("00F0545C-E180-4AD1-8E8A-419061CE505E"), "Razer Tartarus")
                                                                     };

    public static readonly List<(Guid guid, string model)> CHROMALINKS = new()
                                                                         {
                                                                             (new Guid("0201203B-62F3-4C50-83DD-598BABD208E0"), "Core Chroma"),
                                                                             (new Guid("35F6F18D-1AE5-436C-A575-AB44A127903A"), "Lenovo Y900"),
                                                                             (new Guid("47DB1FA7-6B9B-4EE6-B6F4-4071A3B2053B"), "Lenovo Y27"),
                                                                             (new Guid("BB2E9C9B-B0D2-461A-BA52-230B5D6C3609"), "Chroma Box")
                                                                         };

    public static readonly List<(Guid guid, string model)> SPEAKERS = new()
                                                                      {
                                                                          (new Guid("45B308F2-CD44-4594-8375-4D5945AD880E"), "Nommo Chroma"),
                                                                          (new Guid("3017280B-D7F9-4D7B-930E-7B47181B46B5"), "Nommo Chroma Pro")
                                                                      };
}