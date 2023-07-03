﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.EAppOwnershipFlags
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System;

namespace Steamworks
{
  [Flags]
  public enum EAppOwnershipFlags
  {
    k_EAppOwnershipFlags_None = 0,
    k_EAppOwnershipFlags_OwnsLicense = 1,
    k_EAppOwnershipFlags_FreeLicense = 2,
    k_EAppOwnershipFlags_RegionRestricted = 4,
    k_EAppOwnershipFlags_LowViolence = 8,
    k_EAppOwnershipFlags_InvalidPlatform = 16, // 0x00000010
    k_EAppOwnershipFlags_SharedLicense = 32, // 0x00000020
    k_EAppOwnershipFlags_FreeWeekend = 64, // 0x00000040
    k_EAppOwnershipFlags_RetailLicense = 128, // 0x00000080
    k_EAppOwnershipFlags_LicenseLocked = 256, // 0x00000100
    k_EAppOwnershipFlags_LicensePending = 512, // 0x00000200
    k_EAppOwnershipFlags_LicenseExpired = 1024, // 0x00000400
    k_EAppOwnershipFlags_LicensePermanent = 2048, // 0x00000800
    k_EAppOwnershipFlags_LicenseRecurring = 4096, // 0x00001000
    k_EAppOwnershipFlags_LicenseCanceled = 8192, // 0x00002000
    k_EAppOwnershipFlags_AutoGrant = 16384, // 0x00004000
    k_EAppOwnershipFlags_PendingGift = 32768, // 0x00008000
    k_EAppOwnershipFlags_RentalNotActivated = 65536, // 0x00010000
    k_EAppOwnershipFlags_Rental = 131072, // 0x00020000
  }
}
