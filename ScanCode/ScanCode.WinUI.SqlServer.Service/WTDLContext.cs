namespace ScanCode.WinUI.Service
{
    //public partial class WTDLContext : DbContext
    //{
    //    // private static ConnectionMultiplexer _redis;

    //    /// <summary>
    //    /// ���涩��״̬��key��
    //    /// </summary>
    //    private readonly string OrderStatus = "ORDERSSTATUS";

    //    /// <summary>
    //    /// ������ȡ���״̬��KEY���������ʱ��Ϊ24Сʱ
    //    /// </summary>
    //    private readonly string RedPacketKey = "REDPACKETKEY";

    //    public WTDLContext()
    //        : base("name=WTDLModelString")
    //    {
    //        //if (_redis is null)
    //        //{
    //        //    _redis = ConnectionMultiplexer.Connect(ConfigurationManager.ConnectionStrings["RedisConnectionString"].ConnectionString);
    //        //}
    //    }

    //    public virtual DbSet<User> Users { get; set; }
    //    public virtual DbSet<W_LabelStorage> W_LabelStorages { get; set; }
    //    public virtual DbSet<W_OrderTable> W_OrderTables { get; set; }
    //    public virtual DbSet<tAgent> tAgents { get; set; }
    //    //public virtual DbSet<tLabels_Re> tLabels_Res { get; set; }

    //    // public virtual DbSet<tLabels_X> tLabels_Xs { get; set; }

    //    /// <summary>
    //    /// �������״̬
    //    /// </summary>
    //    /// <param name="qrcode"></param>
    //    /// <param name="status"></param>
    //    /// <returns></returns>
    //    /// <exception cref="NotImplementedException"></exception>
    //    //public virtual async Task<bool> SetOrderStatusAsync(string qrcode)
    //    //{
    //    //    var redisdb = _redis.GetDatabase(); //RedisClientFactory.GetDatabase();
    //    //    //��ȡqrcode �е�ƫ����
    //    //    var offset = qrcode.Substring(4, 7);

    //    //    var key = qrcode.Substring(0, 4);

    //    //    //�ж��Ƿ���ڸ�key
    //    //    if (!redisdb.KeyExists(key))
    //    //    {
    //    //        /*ʹ�� BITFIELD �����ʼ��һ����Ϊ [key] ��λͼ��ʹ�� CREATE ������ָ������һ���µ�λͼ��
    //    //         ʹ�� u32 ���ͱ�ʾλͼʹ�� 32 λ�����洢��ʹ�� #10000000 ��ʾλͼ�Ĵ�СΪ 10000000��
    //    //        ִ����ɺ�λͼ�ĳ�ʼ״̬ȫ��Ϊ 0��*/
    //    //        // redisdb.Execute("BITFIELD", key, "CREATE", "u32", "#10000000");
    //    //    }
    //    //    ///����λͼ״̬Ϊtrue
    //    //    return await redisdb.StringSetBitAsync(key, Convert.ToInt64(offset), true);
    //    //}

    //    /// <summary>
    //    /// �������Ĺ���ʱ�䣬���ʹ�á����ⳬ��24Сʱ�ſ�����ȡ���
    //    /// </summary>
    //    /// <param name="qrcode"></param>
    //    /// <returns></returns>
    //    //public virtual async Task<bool> SetRedPacketAsync(string qrcode)
    //    //{
    //    //    var redisdb = _redis.GetDatabase(); //RedisClientFactory.GetDatabase();
    //    //    return await redisdb.StringSetAsync(qrcode, DateTime.Now.ToString(), TimeSpan.FromHours(24));
    //    //}

    //    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //    {
    //        modelBuilder.Entity<User>()
    //            .Property(e => e.UserID)
    //            .IsFixedLength();

    //        modelBuilder.Entity<User>()
    //            .Property(e => e.UserName)
    //            .IsFixedLength();

    //        modelBuilder.Entity<User>()
    //            .Property(e => e.PWD)
    //            .IsFixedLength();

    //        modelBuilder.Entity<User>()
    //            .Property(e => e.AgentID)
    //            .IsFixedLength();

    //        modelBuilder.Entity<W_LabelStorage>()
    //            .Property(e => e.QRCode)
    //            .IsFixedLength();

    //        modelBuilder.Entity<W_LabelStorage>()
    //            .Property(e => e.Adminaccount)
    //            .IsFixedLength();

    //        modelBuilder.Entity<W_LabelStorage>()
    //            .Property(e => e.OutType)
    //            .IsFixedLength();

    //        modelBuilder.Entity<W_LabelStorage>()
    //            .Property(e => e.OrderNumbels)
    //            .IsFixedLength();

    //        modelBuilder.Entity<W_LabelStorage>()
    //            .Property(e => e.ExtensionName)
    //            .IsFixedLength();

    //        modelBuilder.Entity<W_LabelStorage>()
    //            .Property(e => e.ExtensionOrder)
    //            .IsFixedLength();

    //        modelBuilder.Entity<W_OrderTable>()
    //            .Property(e => e.OrderId)
    //            .IsFixedLength();

    //        modelBuilder.Entity<W_OrderTable>()
    //            .Property(e => e.Accounts)
    //            .IsFixedLength();

    //        modelBuilder.Entity<tAgent>()
    //            .Property(e => e.AID)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tAgent>()
    //            .Property(e => e.AName)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tAgent>()
    //            .Property(e => e.AProvince)
    //            .IsFixedLength();

    //        modelBuilder.Entity<tAgent>()
    //            .Property(e => e.ACity)
    //            .IsFixedLength();

    //        modelBuilder.Entity<tAgent>()
    //            .Property(e => e.AAddr)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tAgent>()
    //            .Property(e => e.ATel)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tAgent>()
    //            .Property(e => e.APeople)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tAgent>()
    //            .Property(e => e.ABelong)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_Re>()
    //            .Property(e => e.XCode)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_Re>()
    //            .Property(e => e.PCode)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_Re>()
    //            .Property(e => e.PName)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_Re>()
    //            .Property(e => e.bzStock)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_Re>()
    //            .Property(e => e.bzPt)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_Re>()
    //            .Property(e => e.bzPici)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_Re>()
    //            .Property(e => e.Content)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_Re>()
    //            .Property(e => e.fhRept1)
    //            .IsFixedLength();

    //        modelBuilder.Entity<tLabels_Re>()
    //            .Property(e => e.fhAgent1)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_Re>()
    //            .Property(e => e.fhPt1)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_Re>()
    //            .Property(e => e.fhStock1)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_Re>()
    //            .Property(e => e.fhType1)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_Re>()
    //            .Property(e => e.fhPaper1)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_Re>()
    //            .Property(e => e.Content1)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_Re>()
    //            .Property(e => e.fhAgent2)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_Re>()
    //            .Property(e => e.fhPt2)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_Re>()
    //            .Property(e => e.fhStock2)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_Re>()
    //            .Property(e => e.fhType2)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_Re>()
    //            .Property(e => e.fhPaper2)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_Re>()
    //            .Property(e => e.Content2)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_Re>()
    //            .Property(e => e.fhAgent3)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_Re>()
    //            .Property(e => e.fhPt3)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_Re>()
    //            .Property(e => e.fhStock3)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_Re>()
    //            .Property(e => e.fhType3)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_Re>()
    //            .Property(e => e.fhPaper3)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_Re>()
    //            .Property(e => e.Content3)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_Re>()
    //            .Property(e => e.chPt)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_Re>()
    //            .Property(e => e.chAddr)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_X>()
    //            .Property(e => e.XCode)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_X>()
    //            .Property(e => e.PCode)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_X>()
    //            .Property(e => e.PName)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_X>()
    //            .Property(e => e.bzStock)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_X>()
    //            .Property(e => e.bzPt)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_X>()
    //            .Property(e => e.bzPici)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_X>()
    //            .Property(e => e.Content)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_X>()
    //            .Property(e => e.fhAgent1)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_X>()
    //            .Property(e => e.fhPt1)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_X>()
    //            .Property(e => e.fhStock1)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_X>()
    //            .Property(e => e.fhType1)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_X>()
    //            .Property(e => e.fhPaper1)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_X>()
    //            .Property(e => e.Content1)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_X>()
    //            .Property(e => e.fhAgent2)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_X>()
    //            .Property(e => e.fhPt2)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_X>()
    //            .Property(e => e.fhStock2)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_X>()
    //            .Property(e => e.fhType2)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_X>()
    //            .Property(e => e.fhPaper2)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_X>()
    //            .Property(e => e.Content2)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_X>()
    //            .Property(e => e.fhAgent3)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_X>()
    //            .Property(e => e.fhPt3)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_X>()
    //            .Property(e => e.fhStock3)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_X>()
    //            .Property(e => e.fhType3)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_X>()
    //            .Property(e => e.fhPaper3)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_X>()
    //            .Property(e => e.Content3)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_X>()
    //            .Property(e => e.chPt)
    //            .IsFixedLength()
    //            .IsUnicode(false);

    //        modelBuilder.Entity<tLabels_X>()
    //            .Property(e => e.chAddr)
    //            .IsFixedLength()
    //            .IsUnicode(false);
    //    }
    //}
}