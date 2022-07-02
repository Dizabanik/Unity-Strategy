using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Building
{
    private Resources _resources;
    public int MoneyCount = 1;
    public int PeriodOfMine = 1;

    public override void Start()
    {
        base.Start();
        _resources = FindObjectOfType<Resources>();
    }

    public override void Builded()
    {
        base.Builded();
        InvokeRepeating(nameof(AddMoney), PeriodOfMine, PeriodOfMine);
    }

    private void AddMoney()
    {
        _resources.Money += MoneyCount;
    }
}
