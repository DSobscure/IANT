using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IANTProtocol
{
    public enum GetConfigurationsResponseParameterCode : byte
    {
        TowerUpgradeConfigurationXMLString
    }
    public enum LoginResponseParameterCode : byte
    {
        UniqueID,
        Level,
        EXP
    }
}
