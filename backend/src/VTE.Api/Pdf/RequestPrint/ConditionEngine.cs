namespace VTE.Api.Pdf.RequestPrint;

// Ports the show/hide logic from the legacy printPlav.vb / printBel.vb / printZelen.vb
// `SetupX()` methods. Returns a name -> bool override map; the renderer uses this
// to override layout.Visible at render time.
public static class ConditionEngine
{
    public static Dictionary<string, bool> Plav(PrintRequestData d)
    {
        var v = new Dictionary<string, bool>();

        // Customer kind
        v["PanelCustomerPravno"]   = d.NewOwner.IsCompany;
        v["PanelCustomerFisicko"]  = !d.NewOwner.IsCompany;
        v["lblMBCompany"]          = d.NewOwner.IsCompany;

        // Previous registration panel
        v["panelPreviosRegistrationOwner"] = d.IsPreviosRegistrationReqired
            || ContainsAny(d.RequestTypeName, "повторна");

        // Type checkboxes
        v["CheckBoxPrvPat"]         = ContainsAny(d.RequestTypeName, "по прв пат");
        v["CheckBoxPovtorno"]       = ContainsAny(d.RequestTypeName, "повторна");
        v["CheckBoxPrivremeno"]     = ContainsAny(d.RequestTypeName, "привремена");
        v["CheckBoxTehnickiPromeni"]= ContainsAny(d.RequestTypeName, "технички");

        // Vehicle category (BR-CAT ZelenMap)
        if (d.ZelenCategoryMap.HasValue)
            v[$"CheckBoxVehicleCategory{d.ZelenCategoryMap.Value}"] = true;

        // Power source
        if (d.EnginePowerSourceId is > 0)
            v[$"CheckBoxPowerSource{d.EnginePowerSourceId.Value}"] = true;

        // First registration suppression (legacy hid labels 3/4/5 if FirstRegistration == "ПОВТОРНА")
        if (string.Equals(d.CurrentVehicle.FirstRegistration, "ПОВТОРНА", StringComparison.OrdinalIgnoreCase))
        {
            v["XrLabel3"] = false;
            v["XrLabel4"] = false;
            v["XrLabel5"] = false;
        }

        return v;
    }

    public static Dictionary<string, bool> Bel(PrintRequestData d)
    {
        var v = new Dictionary<string, bool>();

        v["PanelPravnoLice"] = d.NewOwner.IsCompany;
        v["PanelOwner"]      = !d.NewOwner.IsCompany;

        if (d.BelCategoryMap.HasValue)
            v[$"BelVehicelCategory{d.BelCategoryMap.Value}"] = true;

        // Group of labels that show when new customer is part of the request
        var showNewCustomerLabels = d.IsNewCustomer && ContainsAny(d.RequestTypeName, "повторна");
        foreach (var n in new[] { "XrLabel1", "XrLabel2", "XrLabel3", "XrLabel4", "XrLabel5", "XrLabel6", "XrLabel7", "XrLabel9" })
            v[n] = showNewCustomerLabels;

        v["XrLabel38"] = d.IsNewCustomer;

        // CheckBoxDocTyprOption{first-char of TypeName}
        if (d.RequestTypeName.Length > 0)
            v[$"CheckBoxDocTyprOption{d.RequestTypeName[0]}"] = true;

        // First registration place suppression
        if (string.IsNullOrEmpty(d.CurrentVehicle.FirstRegistrationPlace))
        {
            v["XrLabel21"] = false;
            v["XrLabel18FirstRegPlace"] = false;
        }

        return v;
    }

    public static Dictionary<string, bool> Zelen(PrintRequestData d)
    {
        var v = new Dictionary<string, bool>();
        var disp = d.RequestTypeDisplayText;

        // Top-level letter checkboxes А/Б/В/Г
        if (disp.Contains("А -")) v["CheckBoxA"] = true;
        if (disp.Contains("Б -")) v["CheckBoxB"] = true;
        if (disp.Contains("В -")) v["CheckBoxV"] = true;
        if (disp.Contains("Г -")) v["CheckBoxG"] = true;

        // A21..A24
        if (disp.Contains("А21 -")) v["CheckBoxA31"] = true;
        if (disp.Contains("А22 -")) v["CheckBoxA32"] = true;
        if (disp.Contains("А23 -")) v["CheckBoxA33"] = true;
        if (disp.Contains("А24 -")) v["CheckBoxA34"] = true;

        // V1..V9
        for (var i = 1; i <= 9; i++)
            if (disp.Contains($"В{i} -")) v[$"CheckBoxV{i}"] = true;

        // Vehicle category
        if (d.ZelenCategoryMap.HasValue)
            v[$"VehicelCategory{d.ZelenCategoryMap.Value}"] = true;

        v["CheckBoxIsCompany"]    = d.NewOwner.IsCompany;
        v["CheckBoxIsNotCompany"] = !d.NewOwner.IsCompany;

        v["panelVozilo"]    = d.IsVehicleChanged;
        v["PanelCustomer"]  = d.IsCustomerChanged;
        v["lblLastRegistration"] = d.IsNewRegistration;
        v["lblIsReady"]     = true;

        return v;
    }

    private static bool ContainsAny(string haystack, params string[] needles)
    {
        foreach (var n in needles)
            if (haystack.Contains(n, StringComparison.OrdinalIgnoreCase)) return true;
        return false;
    }
}
