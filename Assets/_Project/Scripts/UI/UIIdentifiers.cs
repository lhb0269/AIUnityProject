namespace MobileGame.UI
{
    /// <summary>
    /// 버튼 ID 상수 정의
    /// 테스트와 프로덕션 코드 모두에서 사용하여 UI 리팩토링 시 테스트 깨짐 방지
    /// </summary>
    public static class ButtonID
    {
        // 메뉴 시스템
        public const string HamburgerMenu = "HamburgerMenu";
        public const string Setting = "Setting";

        // 정보 시스템
        public const string UserInfo = "UserInfo";
        public const string GuideQuest = "GuideQuest";

        // 상점/협력/이벤트
        public const string Shop = "Shop";
        public const string Recruitment = "Recruitment";
        public const string Event = "Event";

        // 전투 관련
        public const string Character = "Character";
        public const string SkillSetting = "SkillSetting";
        public const string Skill1 = "Skill1";
        public const string Skill2 = "Skill2";
        public const string Skill3 = "Skill3";
        public const string Skill4 = "Skill4";
        public const string Skill5 = "Skill5";
        public const string Skill6 = "Skill6";
        public const string Weapon = "Weapon";
        public const string Equip = "Equip";
        public const string Coworker = "Coworker";
        public const string Jump = "Jump";
        public const string CoworkerSpawn = "CoworkerSpawn";

        // 아이템
        public const string HPPotion = "HPPotion";
        public const string MPPotion = "MPPotion";
        public const string PotionSetting = "PotionSetting";

        // 게임플레이 컨트롤
        public const string Control = "Control";
        public const string Chapter = "Chapter";
        public const string MonsterSpawn = "MonsterSpawn";
        public const string SpawnSetting = "SpawnSetting";
        public const string ContinuousSpawn = "ContinuousSpawn";

        // 추가 기능
        public const string QuickHunt = "QuickHunt";
        public const string AutoResult = "AutoResult";
        public const string Booster = "Booster";
        public const string GrowUpGuide = "GrowUpGuide";
        public const string Quest = "Quest";
        public const string Chatting = "Chatting";
    }

    /// <summary>
    /// 팝업 ID 상수 정의
    /// UIManager.ShowPopup() 호출 시 사용
    /// </summary>
    public static class PopupID
    {
        public const string HamburgerMenu = "HamburgerMenuPopup";
        public const string Character = "CharacterPopup";
        public const string Shop = "ShopPopup";
        public const string Settings = "SettingsPopup";
        public const string UserInfo = "UserInfoPopup";
        public const string GuideQuest = "GuideQuestPopup";
        public const string Recruitment = "RecruitmentPopup";
        public const string Event = "EventPopup";
        public const string Weapon = "WeaponPopup";
        public const string Equipment = "EquipmentPopup";
        public const string Coworker = "CoworkerPopup";
        public const string SkillSetting = "SkillSettingPopup";
        public const string PotionSetting = "PotionSettingPopup";
        public const string Chapter = "ChapterPopup";
        public const string SpawnSetting = "SpawnSettingPopup";
        public const string QuickHunt = "QuickHuntPopup";
        public const string AutoResult = "AutoResultPopup";
        public const string Booster = "BoosterPopup";
        public const string ContinuousSpawn = "ContinuousSpawnPopup";
        public const string GrowUpGuide = "GrowUpGuidePopup";
        public const string Quest = "QuestPopup";
        public const string Chatting = "ChattingPopup";
        public const string GameSetting = "GameSettingPopup";
        public const string Notice = "NoticePopup";
        public const string Town = "TownPopup";
    }
}
