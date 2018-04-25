public class Achievement : CsvBase
{
// replace start
	/// <summary>
	/// 上一级（同type）
	/// </summary>
	public int prev;

	/// <summary>
	/// 下一级（同type）
	/// </summary>
	public int next;

	/// <summary>
	/// 成就排序id
	/// </summary>
	public int achievement_rank_id;

	/// <summary>
	/// 成就归类给客户端表现用（11是成长、12是收集、13是战斗、14是日常类、15是积累类）
	/// </summary>
	public int achievement_type;

	/// <summary>
	/// 成就类型icon
	/// </summary>
	public string achievement_type_icon;

	/// <summary>
	/// 成就归类名称
	/// </summary>
	public string achievement_type_name;

	/// <summary>
	/// 成就归类名称自己看
	/// </summary>
	public string achievement_type_name_mylook;

	/// <summary>
	/// 成就类型：成就归类+XXX（001是格斗家、002是装备、003是技能、4是天赋点、5是基因、6是战队养成、8是称号、9是普通副本、10是精英副本、11是竞技场、12是天梯挑战、13是爬塔、14是神秘商店、15是签到、16是点金手、17是金币、18是钻石、19是竞技币、20是爬塔币、21是天梯币、22是公会币、23是关卡星数、24是日常任务、25是商店、26是公会）
	/// </summary>
	public int achievement_subType;

	/// <summary>
	/// 成就类型名称
	/// </summary>
	public string achievement_subType_name;

	/// <summary>
	/// 成就类型名称自己看
	/// </summary>
	public string achievement_subType_name_mylook;

	/// <summary>
	/// 是否需要进度条(1是需要，0是不需要)
	/// </summary>
	public bool is_need_article;

	/// <summary>
	/// 细分类型，服务器用细分
	/// </summary>
	public int achievement_detailType;

	/// <summary>
	/// 细分类型名称-成就名字
	/// </summary>
	public string achievement_name;

	/// <summary>
	/// 细分类型名称-成就名字
	/// </summary>
	public string achievement_name_ex;

	/// <summary>
	/// 细分类型名称
	/// </summary>
	public string achievement_detailType_name;

	/// <summary>
	/// 细分类型名称自己看
	/// </summary>
	public string achievement_detailType_name_mylook;

	/// <summary>
	/// 计数器id（索引counter表）
	/// </summary>
	public int counterid;

	/// <summary>
	/// 条件（章节进度用该章节最后一个关卡id）
	/// </summary>
	public int achievement_condition;

	/// <summary>
	/// 条件（前端用）
	/// </summary>
	public int achievement_condition_client;

	/// <summary>
	/// 成就奖励：id（索引item）与数量)
	/// </summary>
	public string[] achievement_item_award;

	/// <summary>
	/// 成就icon
	/// </summary>
	public string achievement_icon;

// replace end
}

