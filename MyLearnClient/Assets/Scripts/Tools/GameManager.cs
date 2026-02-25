using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBase<GameManager>
{
    

    public SoldierBase CreatSoldier(int id)
    {
        switch (id)
        {
            case 0:
                return new BuBingSoldier();
            case 1:
                return new QiBingSoldier();
        }
        return null;
    }
}
