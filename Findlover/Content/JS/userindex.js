$(document).ready(function () {
    var baseinfo;
    var requirement;
    //获取json baseinfo
    function getBaseInfoJson() {
        $.post("/User/GetBaseinfo", function (data) {
            $("#ResidenceProvince").val(data.ResidenceProvince);
            $("#ResidenceProvince").change();
            $("#ResidenceCity").val(data.ResidenceCity);
            $("#Height").val(data.Height);
            $("#MonthlyIncome").val(data.MonthlyIncome);
            $("#Education").val(data.Education);
            $("#Company").val(data.Company);
            $("#Position").val(data.Position);
            $("#State").val(data.State);
            

        });
    }
    function getDetailInfoJson() {
        $.post("/User/GetDetailInfo", function (data) {
            $("#Provience").val(data.NativePlace);
            $("#Provience").change();
            $("#City").val(data.NativeCity);

            $("#Car").val(data.Car);
            $("#Housing").val(data.Housing);
            $("#Weight").val(data.Weight);
            $("#People").val(data.People);
            $("#Constellation").val(data.Constellation);
            $("#BloodType").val(data.BloodType);
            $("#DouBan").val(data.DouBan);
            $("#MicroBlog").val(data.MicroBlog);
        });
    }
     function getLoveViewJson() {
        $.post("/User/GetLoveView", function (data) {
           
            $("#WantaBaby").val(data.WantaBaby);
            $("#WorkTimePlan").val(data.WorkTimePlan);
            $("#Smoking").val(data.Smoking);
            $("#Drinking").val(data.Drinking);
            $("#LoveDuration").val(data.LoveDuration);
            $("#ParentLiveTogether").val(data.ParentLiveTogether);
            $("#Housework").val(data.Housework);
            $("#ManageMoney").val(data.ManageMoney);
            $("#Cooking").val(data.Cooking);
            $("#Allopatry").val(data.Allopatry);
        });
    }
    
    function getRequirementJson() {
        $.post("/User/GetRequirement", function (data) {
            requirement = data;
            //住的地方
            $("#NowProvience").val(data.ResidenceProvince);
            $("#NowProvience").change();
            $("#NowCity").val(data.ResidenceCity);
            //身高
            var hightl = data.HightLl + "厘米";
            var h2 = data.HightUl + "厘米";
            $("#HeightL").val(hightl);
            $("#HeightL").change();
            $("#HeightH").val(h2);
            //年龄
            var a1 = data.AgeLl + "岁", a2 = data.AgeUl + "岁";
            $("#AgeL").val(a1);
            $("#AgeL").change();
            $("#AgeH").val(a2);
            //学历
            $("#REducation").val(data.Education);
            // 月收入
            var m1 = data.MonthlyIncomeLl+"元", m2 = data.MonthlyIncomeUl+"元";
            $("#SalaryL").val(m1);
            $("#SalaryL").change();
            $("#SalaryH").val(m2);

            initRequirement(data);
        });
    }
    getLoveViewJson();
    getDetailInfoJson();
    getBaseInfoJson();
    getRequirementJson();
    //让select选中用户的的选项。
   
    //获取资料完整度
    function getPersent() {
        $.get("/User/GetPersent", function (data) {
            if (data < 0.3) {
                $("#pinfo").html("资料完整度：不够30%,交互受影响哦");
            }
            if (data >= 0.3 && data < 0.6) {
                $("#pinfo").html("资料完整度：还不错");
            }
            if (data >= 0.6) {
                $("#pinfo").html("资料完整度：灰常不错哦,态度可嘉");
            }
            data = data * 100;
            $(".percent").html(Math.round(data, 2) + "%");
        });
    }
    getPersent();

    //初始化
    $("#HeightL").append("<option>不限</option>");
    for (var i = 150; i < 211; i++) {
        $("#Height,#HeightL").append("<option>" + i + "厘米</option>");    
    }
    
    for (var j = 151; j < 211; j++) {
        $("#HeightH").append("<option>" + j + "厘米</option>");
    }
    for (var l = 18; l< 61; l++) {
        $("#AgeL").append("<option>" + l + "岁</option>");
    }
    for (var h = 18; h < 61; h++) {
        $("#AgeH").append("<option>" + h + "岁</option>");
    }
    
    $("#HeightL").change(function () {
        var value = $(this).val().substring(0, 3);
        value = parseInt(value) + 1;
        $("#HeightH").find("option:gt(0)").remove();
        for (var v = value; v < 210; v++) {
            $("#HeightH").append("<option>" + v + "厘米</option>");
        }
    });
    
    $("#AgeL").change(function () {
        var value = $(this).val().substring(0, 3);
        value = parseInt(value) + 1;
        $("#AgeH").find("option:gt(0)").remove();
        for (var v = value; v < 210; v++) {
            $("#AgeH").append("<option>" + v + "岁</option>");
        }
    });
    $("#SalaryL").change(function() {
        var value = $(this).val();
        $("#SalaryH").find("option:gt(0)").remove();
        switch (value) {
        case "不限":
            break;
            case "3000元":
                appendSlary(0);
                break;
            case "4000元":
                appendSlary(1);
                break;
            case "5000元":
                appendSlary(2);
                break;
            case "6000元":
                appendSlary(3);
                break;
            case "8000元":
                appendSlary(4);
                break;
            case "10000元":
                appendSlary(5);
                break;
         
        default:
        }
    });
    var ins = ['4000元','5000元', '6000元', '8000元', '10000元'];
    function appendSlary(n) {
        for (var k = n; k < 5; k++) {
            $("#SalaryH").append("<option>" + ins[k] + "</option>");
        }
    }
    var edus = ['大专以下', '大专', '本科', '硕士', '博士'];
    var newedus = ['不限', '大专及以上', '本科及以上', '硕士及以上', '博士及以上'];
    var incomes = ['3000元以下','3000-4000元', '4000-5000元', '5000-6000元', '6000-8000元', '8000-10000元',"10000-20000元","20000元以上"];
    var incomeL = ['不限', '3000元','4000元','5000元', '6000元', '8000元', '10000元'];
    var coms = ['中达一厂', '中达二厂', '中达三厂', '中达四厂', '中达五厂', '中达华峰厂', '中达地区公共'];
    var stas = ['未婚,寻觅中', '离异,寻觅中', '丧偶,寻觅中', '已有对象,不在寻觅(关闭资料)'];
    var pos = ['生技', '制造', '测试', '品管', '技工', '资讯', '财务', '物流', '人资', '行政', '后勤', '作业员', '研发', '社区管理', "管理"];
    var hos = ['请选择', '已购房', '租房', '公司宿舍', '和家人同住'];
    var cars = ['请选择', '已购车', '未购车'];
    var boolds = ['请选择', 'A型', 'B型', 'AB型', 'O型'];
    var cons = ['请选择', '摩羯座', '水平座', '双鱼座', '白羊座', '金牛座', '双子座', '巨蟹座', '狮子座', '处女座', '天秤座', '天蝎座', '射手座'];
    var peoples = ['请选择', '汉族', '壮族', '满族', '回族', '苗族', '土家族', '维吾尔族', '彝族', '藏族', '布依族', '侗族', '瑶族', '朝鲜族', '白族', '傣族', '高山族',
    '畲族', '僳僳族', '仡佬族', '东乡族', '水族', '佤族', '羌族', '仫佬族'];
    var pros = ['请选择', '北京', '上海', '天津', '重庆', '黑龙江', '吉林', '山东', '山西', '陕西', '河北', '河南', '湖北', '湖南', '海南', '江苏', '江西', '广东', '广西', '云南', '贵州',
        '四川', '内蒙古', '宁夏', '甘肃', '青海', '安徽', '浙江', '福建', '西藏', '新疆', '台湾', '香港', '澳门'];
    var works = ['请选择', "工作时间固定,不接受对方出差", "工作时间固定,不介意对方出差", "工作经常出差","工作偶尔出差"];
    var somks = ['请选择',"不吸烟,而且反感吸烟","不吸烟,但也不介意","偶尔吸烟","经常吸烟"];
    var dinks = ['请选择',"不喝酒,且很反感喝酒","不喝酒,但也不介意","偶尔社交需要才喝酒","经常喝酒"];
    var duras = ['请选择',"能接受闪婚","一年内","两年内","三年以上","赶紧带我走吧"];
    var babys = ['请选择',"想要孩子","不想要孩子","造人需要夫妻共同计划"];
    var hosws = ['请选择', '是妻子的事情', '平均分配', '谁有时间谁做', '各自承担自己擅长的部分'];
    var monys = ['请选择', '由妻子打理', '由丈夫打理', '夫妻共同计划', '各自支配各自收入'];
    var cooks = ['请选择', '会做饭,希望对方也会', '会做饭,对另一半没要求', '不太会,对另一半没要求', '不太会,希望对方厨艺比我好'];
    var lives = ['请选择', '不介意', '介意'];
    var allrs = ['请选择','不接受','接受','接受,但必须要先在一起过'];

    initSelete($("#WorkTimePlan"),works);
    initSelete($("#Smoking"),somks);
    initSelete($("#Drinking"),dinks);
    initSelete($("#LoveDuration"),duras);
    initSelete($("#WantaBaby"),babys);
    initSelete($("#Housework"),hosws);
    initSelete($("#ManageMoney"),monys);
    initSelete($("#Cooking"),cooks );
    initSelete($("#ParentLiveTogether"),lives);
    initSelete($("#Allopatry"),allrs);
    initSelete($("#Provience"), pros);
    initSelete($("#NowProvience"), pros);
    initSelete($("#ResidenceProvince"), pros);
    $("#NowProvience>option:eq(0)").remove();//地点是必须选择的，且默认指向用户自己的居住地
    $("#ResidenceProvince>option:eq(0)").remove();//地点是必须选择的，且默认指向用户自己的居住地
    initSelete($("#People"), peoples);
    initSelete($("#BloodType"), boolds);
    initSelete($("#Constellation"), cons);
    initSelete($("#Car"), cars);
    initSelete($("#Housing"), hos);
    initSelete($("#Education"), edus);
    initSelete($("#REducation"), newedus);
    initSelete($("#MonthlyIncome"), incomes);
    initSelete($("#SalaryL"), incomeL);
    initSelete($("#Company"), coms);
    initSelete($("#State"), stas);
    initSelete($("#Position"), pos);


    function initSelete(sl, strs) {
        sl.empty();
        for (var a = 0; a < strs.length; a++) {
            sl.append("<option>" + strs[a] + "</option>");
        }
    }

    var beijing = ['选择城市','东城区', '西城区', '崇文区', '宣武区', '朝阳区', '丰台区', '石景山区', '海淀区', '门头沟区', '房山区', '通州区', '顺义区', '昌平区', '大兴区',
        '平谷区', '怀柔区', '密云县', '延庆县'];
    var shanghai = ['请选择', '黄浦区', '卢湾区', '徐汇区', '长宁区', '静安区', '普陀区', '闸北区', '虹口区', '杨浦区', '闵行区', '宝山区', '嘉定区', '浦东新区', '金山区',
        '松江区', '青浦区', '南汇区', '奉贤区', '崇明县'];
    var tianjin = ['选择城市', '和平区', '河东区', '河西区', '南开区', '河北区', '红桥区', '塘沽区', '汉沽区', '大港区', '东丽区', '西青区', '津南区', '北辰区', '武清区', '宝坻区', '宁河县', '静海县', '蓟县'];
    var chongqing = ('选择城市 万州区 涪陵区 渝中区 大渡口区 江北区 沙坪坝区 九龙坡区 南岸区 北碚区 万盛区 双桥区 渝北区 巴南区 黔江区 长寿区 江津区 合川区 永川区 南川区 綦江县 潼南县 铜梁县 大足县 荣昌县 璧山县 梁平县 城口县 丰都县 垫江县 ' +
        '武隆县 忠县 开县 云阳县 奉节县 巫山县 巫溪县 石柱土家族自治县 秀山土 家族苗族自治县 酉阳土家族苗族自治县 彭水苗族土家族自治县').split(' ');
    var heilongjiang = ('选择城市 哈尔滨市 齐齐哈尔市 鸡西市 鹤岗市 双鸭山市 大庆市 伊春市 佳木斯市 七台河市 牡丹江市 黑河市 绥化市 大兴安岭地区').split(' ');
    var jilin = ('选择城市 长春市 吉林市 四平市 辽源市 通化市 白山市 松原市 白城市 延边朝鲜族自治州 辽宁 沈阳市 大连市 鞍山市 抚顺市 本溪市 丹东市 锦州市 营口市 阜新市 辽阳市 盘锦市 铁岭市 朝阳市 葫芦岛市').split(' ');
    var shandong = '选择城市 济南市 青岛市 淄博市 枣庄市 东营市 烟台市 潍坊市 济宁市 泰安市 威海市 日照市 莱芜市 临沂市 德州市 聊城市 滨州市 菏泽市'.split(' ');
    var shanxi = '选择城市 太原市 大同市 阳泉市 长治市 晋城市 朔州市 晋中市 运城市 忻州市 临汾市 吕梁市'.split(' ');
    var shanxiX = '选择城市 西安市 铜川市 宝鸡市 咸阳市 渭南市 延安市 汉中市 榆林市 安康市 商洛市'.split(' ');
    var hebei = '选择城市 石家庄市 唐山市 秦皇岛市 邯郸市 邢台市 保定市 张家口市 承德市 沧州市 廊坊市 衡水市'.split(' ');
    var henan = '选择城市 郑州市 开封市 洛阳市 平顶山市 安阳市 鹤壁市 新乡市 焦作市 济源市 濮阳市 许昌市 漯河市 三门峡市 南阳市 商丘市 信阳市 周口市 驻马店市'.split(' ');
    var hubei = '选择城市 武汉市 黄石市 十堰市 宜昌市 襄樊市 鄂州市 荆门市 孝感市 荆州市 黄冈市 咸宁市 随州市 恩施土家族苗族自治州 仙桃市 潜江市 天门市 神农架林区'.split(' ');
    var hunan = '选择城市 长沙市 株洲市 湘潭市 衡阳市 邵阳市 岳阳市 常德市 张家界市 益阳市 郴州市 永州市 怀化市 娄底市 湘西土家族苗族自治州'.split(' ');
    var hainan = '选择城市 海口市 三亚市 五指山市 琼海市 儋州市 文昌市 万宁市 东方市 定安县 屯昌县 澄迈县 临高县 白沙黎族自治县 昌江黎族自治县 乐东黎族自治县 陵水黎族自治县 保亭黎族苗族自治县 琼中黎族苗族自治县'.split(' ');
    var jiangsu = '选择城市 南京市 无锡市 徐州市 常州市 苏州市 南通市 连云港市 淮安市 盐城市 扬州市 镇江市 泰州市 宿迁市'.split(' ');
    var jiangxi = '选择城市 南昌市 景德镇市 萍乡市 九江市 新余市 鹰潭市 赣州市 吉安市 宜春市 抚州市 上饶市'.split(' ');
    var guangdong = '选择城市 广州市 韶关市 深圳市 珠海市 汕头市 佛山市 江门市 湛江市 茂名市 肇庆市 惠州市 梅州市 汕尾市 河源市 阳江市 清远市 东莞市 中山市 潮州市 揭阳市 云浮市'.split(' ');
    var guangxi = '选择城市 南宁市 柳州市 桂林市 梧州市 北海市 防城港市 钦州市 贵港市 玉林市 百色市 贺州市 河池市 来宾市 崇左市'.split(' ');
    var yunnan = '选择城市 昆明市 曲靖市 玉溪市 保山市 昭通市 丽江市 思茅市 临沧市 楚雄彝族自治州 红河哈尼族彝族自治州 文山壮族苗族自治州 西双版纳傣族自治州 大理白族自治州 德宏傣族景颇族自治州 怒江傈僳族自治州 迪庆藏族自治州'.split(' ');
    var guizhou = '选择城市 贵阳市 六盘水市 遵义市 安顺市 铜仁地区 黔西南布依族苗族自治州 毕节地区 黔东南苗族侗族自治州 黔南布依族苗族自治州'.split(' ');
    var sichuan = '选择城市 成都市 自贡市 攀枝花市 泸州市 德阳市 绵阳市 广元市 遂宁市 内江市 乐山市 南充市 眉山市 宜宾市 广安市 达州市 雅安市 巴中市 资阳市 阿坝藏族羌族自治州 甘孜藏族自治州 凉山彝族自治州'.split(' ');
    var neimenggu = '选择城市 呼和浩特市 包头市 乌海市 赤峰市 通辽市 鄂尔多斯市 呼伦贝尔市 巴彦淖尔市 乌兰察布市 兴安盟 锡林郭勒盟 阿拉善盟'.split(' ');
    var ningxia = '选择城市 银川市 石嘴山市 吴忠市 固原市 中卫市'.split(' ');
    var qignhai = '选择城市 西宁市 海东地区 海北藏族自治州 黄南藏族自治州 海南藏族自治州 果洛藏族自治州 玉树藏族自治州 海西蒙古族藏族自治州'.split(' ');
    var gansu = '选择城市 兰州市 嘉峪关市 金昌市 白银市 天水市 武威市 张掖市 平凉市 酒泉市 庆阳市 定西市 陇南市 临夏回族自治州 甘南藏族自治州'.split(' ');
    var anhui = '选择城市 合肥市 芜湖市 蚌埠市 淮南市 马鞍山市 淮北市 铜陵市 安庆市 黄山市 滁州市 阜阳市 宿州市 巢湖市 六安市 亳州市 池州市 宣城市'.split(' ');
    var zhejiang = '选择城市 杭州市 宁波市 温州市 嘉兴市 湖州市 绍兴市 金华市 衢州市 舟山市 台州市 丽水市'.split(' ');
    var fujian = '选择城市 福州市 厦门市 莆田市 三明市 泉州市 漳州市 南平市 龙岩市 宁德市'.split(' ');
    var xizang = '选择城市 拉萨市 昌都地区 山南地区新疆 乌鲁木齐市 克拉玛依市 吐鲁番地区 哈密地区日喀则地区 那曲地区 阿里地区 林芝地区 昌吉回族自治州 博尔塔拉蒙古自治州 巴音郭楞蒙古自治州 阿克苏地区 克孜勒苏柯尔克孜自治州 喀什地区 和田地区 伊犁哈萨克自治州 塔城地区 阿勒泰地区 石河子市 阿拉尔市 图木舒克市 五家渠市'.split(' ');
    var xinjiang = ['选择城市','乌鲁木齐市', '克拉玛依市', '吐鲁番地区', '哈密地区', '昌吉回族自治州', '博尔塔拉蒙古自治州', '巴音郭楞蒙古自治州', '阿克苏地区', '克孜勒苏柯尔克孜自治州',
        '喀什地区', '和田地区', '伊犁哈萨克自治州', '塔城地区', '阿勒泰地区', '省直辖行政单位'];
    var taiwan = '选择城市 台北市 高雄市 基隆市 台中市 台南市 新竹市 嘉义市'.split(' ');
    var xianggang = '选择城市 中西区 湾仔区 东区 南区 油尖旺区 深水埗区 九龙城区 黄大仙区 观塘区 荃湾区 葵青区 沙田区 西贡区 大埔区 北区 元朗区 屯门区 离岛区'.split(' ');
    var aomen = ['选择城市', '澳门'];

    //,#NowProvience,#ResidenceProvince  $("#NativePlace").val(pro); $("#City,#ResidenceCity").empty();
    $("#Provience").change(function () {
        var pro = $(this).val();
        swichCity($("#City"), pro);
    });
    $("#ResidenceProvince").change(function () {
        var pro = $(this).val();
        swichCity($("#ResidenceCity"), pro);
    });
    $("#NowProvience").change(function () {
        var pro = $(this).val();
        swichCity($("#NowCity"), pro);
    });

    function swichCity($city, city) {
        switch (city) {
            case "请选择":
                $city.empty();
                break;
            case "北京":
                initSelete($city, beijing);
                break;
            case "上海":
                initSelete($city, shanghai);
                break;
            case "天津":
                initSelete($city, tianjin);
                break;
            case "重庆":
                initSelete($city, chongqing);
                break;
            case "黑龙江":
                initSelete($city, heilongjiang);
                break;
            case "吉林":
                initSelete($city, jilin);
                break;
            case "山东":
                initSelete($city, shandong);
                break;
            case "山西":
                initSelete($city, shanxi);
                break;
            case "陕西":
                initSelete($city, shanxiX);
                break;
            case "河北":
                initSelete($city, hebei);
                break;
            case "河南":
                initSelete($city, henan);
                break;
            case "湖北":
                initSelete($city, hubei);
                break;
            case "湖南":
                initSelete($city, hunan);
                break;
            case "海南":
                initSelete($city, hainan);
                break;
            case "江苏":
                initSelete($city, jiangsu);
                break;
            case "江西":
                initSelete($city, jiangxi);
                break;
            case "广东":
                initSelete($city, guangdong);
                break;
            case "广西":
                initSelete($city, guangxi);
                break;
            case "云南":
                initSelete($city, yunnan);
                break;
            case "四川":
                initSelete($city, sichuan);
                break;
            case "内蒙古":
                initSelete($city, neimenggu);
                break;
            case "宁夏":
                initSelete($city, ningxia);
                break;
            case "贵州":
                initSelete($city, guizhou);
                break;
            case "甘肃":
                initSelete($city, gansu);
                break;
            case "青海":
                initSelete($city, qignhai);
                break;
            case "安徽":
                initSelete($city, anhui);
                break;
            case "浙江":
                initSelete($city, zhejiang);
                break;
            case "福建":
                initSelete($city, fujian);
                break;
            case "西藏":
                initSelete($city, xizang);
                break;
            case "新疆":
                initSelete($city, xinjiang);
                break;
            case "台湾":
                initSelete($city, taiwan);
                break;
            case "香港":
                initSelete($city, xianggang);
                break;
            case "澳门":
                initSelete($city, aomen);
                break;
            default:
        }
    }


    function checkNull() {
        if (baseinfo.Height == null || baseinfo.Height == '') {
            $("#Height").append("<option></option>");
        }
        if (baseinfo.Education == null || baseinfo.Education == '') {
            $("#Education").append("<option></option>");
        }
        if (baseinfo.MonthlyIncome == null || baseinfo.MonthlyIncome == '') {
            $("#MonthlyIncome").append("<option></option>");
        }
        if (baseinfo.Company == null || baseinfo.Company == '') {
            $("#Company").append("<option></option>");
        }
        if (baseinfo.ResidenceCity == null || baseinfo.ResidenceCity == '') {
            $("#ResidenceCity").append("<option></option>");
        }
        if (baseinfo.State == null || baseinfo.State == '') {
            $("#State").append("<option></option>");
        }
        if (baseinfo.Position == null || baseinfo.Position == '') {
            $("#Position").append("<option></option>");
        }
    }
  
    //取消编辑
    $(".cancel-eidt").click(function () {
        $(".infowrap").show();
        $(".infoedit").hide();
        getBaseInfoJson();
    });
    //编辑 baseinfo
    var addnum = 0;
    $(".eidt").click(function () {
        $(".infowrap").hide();
        $(".infoedit").show();
        if (addnum == 0) {
            checkNull();
        }
        addnum++;
    });

    function initRequirement(data) {
        var $sbox = $(".sbox").eq(0);
        var spans = $sbox.find("strong");
        spans.eq(0).html(data.ResidenceProvince);
        spans.eq(1).html(data.ResidenceCity);
        spans.eq(2).html(data.AgeLl);
        //处理年龄显示问题
        if (data.AgeUl == 0) {
            var t = $sbox.find("span:eq(1)").html().replace("之间", "以上");
            $sbox.find("span:eq(1)").html(t);
            $("#ageul").hide();
            //alert(data.AgeUl);
        } else {
            $("#ageul").show();
            spans.eq(3).html("到" + data.AgeUl);
            var x = $sbox.find("span:eq(1)").html().replace("以上", "之间");
            $sbox.find("span:eq(1)").html(x);
        }
        //身高问题
        if (data.HightLl == 0) {//说明没有身高条件 直接隐藏这句话
            $sbox.find("span:eq(2)").hide();
        } else {
            spans.eq(4).html(data.HightLl);
            $sbox.find("span:eq(2)").show();
            if (data.HightUl == 0) {
                $sbox.find("span:eq(2)").html($sbox.find("span:eq(2)").html().replace("之间", "以上"));
                $("#heighul").hide();
            } else {
                $("#heighul").show();
                spans.eq(5).html("到" + data.HightUl);
                $sbox.find("span:eq(2)").html($sbox.find("span:eq(2)").html().replace("以上", "之间"));
            }
        }
        //学历
        if (data.Education == "" || data.Education == null) {
            $("#education_show").hide();
        } else {
            $("#education_show").show();
            spans.eq(6).html(data.Education);
        }
        // 月薪
        if (data.MonthlyIncomeLl == 0) {
            $sbox.find("span:eq(4)").hide();
        } else {
            spans.eq(7).html(data.MonthlyIncomeLl);
            $sbox.find("span:eq(4)").show();
            if (data.MonthlyIncomeUl == 0) {
                $sbox.find("span:eq(4)").html($sbox.find("span:eq(4)").html().replace("之间", "以上"));
                $("#slary").hide();
            } else {
                $("#slary").show();
                spans.eq(8).html("到" + data.MonthlyIncomeUl);
                $sbox.find("span:eq(4)").html($sbox.find("span:eq(4)").html().replace("以上", "之间"));
            }
        }
      
    }
 
    //编辑于取消编辑切换
    $(".saytitle span").click(function () {
        $(this).hide();
        $(".saytitle a,.say").show();
        $(".saying").hide();
    });
    $(".saytitle a").click(function () {
        $(this).hide();
        $(".saytitle span").show();
        $(".saying").show();
        $(".saytitle a,.say").hide();
    });


    $(".selecttitle span").each(function (n) {
        $(this).click(function () {
            $(".selecttitle a").eq(n).show();
            $(".eidtbox").eq(n).show();
            $(".sbox").eq(n).hide();
            $(this).hide();
        });
    });
 $(".selecttitle a").each(function (n) {
        $(this).click(function () {
            $(".selecttitle span").eq(n).show();
            $(this).hide();
            $(".sbox").eq(n).show();
            $(".eidtbox").eq(n).hide();
        });
    });

    // 详细资料中 为空则显示--
    iniempty($(".ulright"));

    function iniempty(collection) {
        collection.each(function () {
            if ($(this).html() == "" || $(this).html() == "0" || $(this).html() == "请选择") {
                $(this).html("--");
            }
        });
    }

    //统计独白的字数
    var $tex = $(".say span i");
    var $but = $("#saywhat");
    var ie = jQuery.support.htmlSerialize;
    var str = 0;
    var abcnum = 0;
    var maxNum = 400;
    var texts = 0;
    var total;
    var enable = false;
    if (ie) {
        $but[0].oninput = changeNum;
    } else {
        $but[0].onpropertychange = changeNum;
    }
    function changeNum() {
        //汉字的个数
        str = ($but.val().replace(/\w/g, "")).length;
        //非汉字的个数
        abcnum = $but.val().length - str;
        total = str * 2 + abcnum;
        texts = Math.ceil((maxNum - (total)) / 2);
        if (total > maxNum) {
            $tex.css("color", 'red');
            enable = false;
        } else {
            $tex.css("color", 'black');
            enable = true;
        }
        if (str == 0) {
            enable = false;
        }
        $tex.html(texts);
    }

    $(".say .btn").click(function(e) {
        e.preventDefault();
        if (enable) {
            $.post("/User/SendPersonalState", { "content": $but.val() }, function (data) {
                if (data == 0) {
                    $("#textempty").show().html("你更新太快了,还不到3分钟呢");
                    var terro1 = setTimeout(function () {
                        $("#textempty").fadeOut();
                    }, 3000);
                    terro1.clearTimeout();
                    return;
                }
                $(".saying").html(data);
                $(".saytitle span").show();
                $(".saying").show();
                $(".saytitle a,.say").hide();
            });
        } else {
            if (str == 0) {
                //别忘输入内容哦
                $("#textempty").show().html("别忘输入内容哦");
                var terro = setTimeout(function() {
                    $("#textempty").fadeOut();
                }, 3000);
                terro.clearTimeout();
            }
        }
    });
    var options = {
        dataType: 'json',
        success: processJson,
    };
    var optiondetails = {
        dataType: 'json',
        success: processJson2,
        beforeSend: function () {
        }
    };
    var optionloves = {
        dataType: 'json',
        success: processJson3,
    };
    
    var optionrequirement = {
        dataType: 'json',
        success: processJsonRequirement,
    };
    $('#Form1').submit(function () {
        $(this).ajaxSubmit(options);
        return false;
    });

    $('#Form2').submit(function () {
        $(this).ajaxSubmit(optiondetails);
        return false;
    });
    $('#Form3').submit(function () {
        $(this).ajaxSubmit(optionloves);
        return false;
    });
    $("#FormRequirement").submit(function() {
        $(this).ajaxSubmit(optionrequirement);
        return false;
    });
    function processJsonRequirement(data) {
        $(".eidtbox").eq(0).hide();
        initRequirement(data);
        $(".selecttitle:eq(0) span").show();
        $(".selecttitle:eq(0) a").hide();
        $(".sbox:eq(0)").show();
    }

    //恋爱观
    function processJson3(data) {
        $(".sbox").eq(2).show();
        $(".eidtbox").eq(2).hide();
        getLoveViewJson();
        getPersent();
        var spans = $(".sbox:eq(2) .ulright");
        spans.eq(0).html(data.WorkTimePlan);
        spans.eq(1).html(data.Smoking);
        spans.eq(2).html(data.Drinking);
        spans.eq(3).html(data.LoveDuration);
        spans.eq(4).html(data.WantaBaby);
        spans.eq(5).html(data.ParentLiveTogether);
        spans.eq(6).html(data.Housework);
        spans.eq(7).html(data.ManageMoney);
        spans.eq(8).html(data.Cooking);
        spans.eq(9).html(data.Allopatry);
        iniempty(spans);
        $(".selecttitle:eq(2) span").show();
        $(".selecttitle:eq(2) a").hide();
    }

    //详细资料
    function processJson2(data) {
        $(".sbox").eq(1).show();
        $(".eidtbox").eq(1).hide();
        getDetailInfoJson();
        getPersent();
        var spans = $(".sbox:eq(1) .ulright");
        spans.eq(0).html(data.Housing);
        spans.eq(1).html(data.Car);
        spans.eq(2).html(data.Weight);
        spans.eq(3).html(data.People);
        spans.eq(4).html(data.NativePlace);
        spans.eq(5).html(data.Constellation);
        spans.eq(6).html(data.BloodType);
        spans.eq(7).html(data.MicroBlog);
        spans.eq(8).html(data.DouBan);
        iniempty(spans);
        $(".selecttitle:eq(1) span").show();
        $(".selecttitle:eq(1) a").hide();
    }

    function processJson(data) {
        // 还是返回json的好，再去更新。
        $(".infowrap").show();
        $(".infoedit").hide();
        getPersent();
        //   alert(data.Height);
        $("#sHeight").html(data.Height);
        $("#sCompany").html(data.Company);
        $("#sEducation").html(data.Education);
        $("#sMonthlyIncome").html(data.MonthlyIncome);
        $("#sResidenceCity").html(data.ResidenceProvince+" "+ data.ResidenceCity);
        $("#sProfession").html(data.Profession);
        $("#sPosition").html(data.Position);
        $("#sSchool").html(data.School);
        $("#sState").html(data.State);
    }
    //再更新一遍资料完整度。
    //----------------------------------------tabs
   
    //$('#myTab a').click(function (e) {
    //    e.preventDefault();
    //    $(this).tab('show');
    //});

    //不是本人，隐藏编辑， 出现喜欢和私信。  要考虑加入 黑名单和检举的功能
    
    if ($("#whovisit").val() != "Self") {
        //隐藏图片和资料编辑
        $("#modifyimg,.eidt,.saytitle>span,.selecttitle span").hide();
    } else {
        $(".btnholder").hide();
    }

});






