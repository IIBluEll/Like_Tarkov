/// <summary>
/// 차후 무기의 다양성을 추가하기 위해 공용으로 사용하는 변수들을 구조체로 묶어 정의
/// </summary>

public enum Tps_WeaponName {AussultRifle = 0}

[System.Serializable]
public class TSP_WeaponSetting
{
    public Tps_WeaponName weaponName;
    public float attackRate;        // 공격 속도
    public float attackDistance;    // 공격 사거리
    public bool isAutomaticAttack;  // 연속 공격 여부

    public int currentMagazine;     // 현재 탄창 수
    public int maxMagazine;         // 최대 탄창 수
    
    public int currentAmmo;         // 현재 탄약 수
    public int maxAmmo;             // 최대 탄약 수
}
