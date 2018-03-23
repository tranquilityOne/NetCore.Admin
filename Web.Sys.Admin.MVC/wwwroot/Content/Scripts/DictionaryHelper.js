


var dataAll = new Array();//全部数据，包括子集
function BindDictionary(combotreeId, DicNo) {//自己和子集
    $.ajax({
        url: '/httpHandle/DictionaryHandler.ashx?action=GetSonDictionary&DicNo=' + ValueName,
        type: 'POST',
        cache: false,
        error: function () { alert('获取类别出错'); },
        success: function (result) {
            var Dictionarydata = eval(result);
            pushAll(Dictionarydata);
            $('#' + combotreeId).combotree('loadData', ConvertToTreeData(Dictionarydata));
        }
    });
}
function GetSonDictionaryNo(combotreeId, DicNo) {//除掉本身只要子集
    $.ajax({
        url: '/httpHandle/DictionaryHandler.ashx?action=GetSonDictionaryNo&DicNo=' + ValueName,
        type: 'POST',
        cache: false,
        error: function () { alert('获取类别出错'); },
        success: function (result) {
            var Dictionarydata = eval(result);
            pushAll(Dictionarydata);
            $('#' + combotreeId).combotree('loadData', ConvertToTreeData(Dictionarydata));
        }
    });
}
function GetDictionaryByDicType(combotreeId, DicType) {//根据类型获取 所有的词典，树型结构
    $.ajax({
        url: '/httpHandle/DictionaryHandler.ashx?action=GetDictionaryByDicType&DicType=' + DicType,
        type: 'POST',
        cache: false,
        error: function () { alert('获取类别出错'); },
        success: function (result) {
            var Dictionarydata = eval(result);
            pushAll(Dictionarydata);
            $('#' + combotreeId).combotree('loadData', ConvertToTreeData(Dictionarydata));
        }
    });
}


//取名
function GetNameByValue(value) {
    var ss = "";
    $.each(dataAll, function (i, n) {
        if (n.id == value) {
            ss = n.Name;
            return false;
        }
    })
    return ss;

}
//取出数据
function pushAll(obj) {
    dataAll = new Array();//全部数据，包括子集
    $.each(obj, function (i, n) {
        pushSon(n);
    })

}
function pushSon(obj) {
    dataAll.push(obj);
    if (obj.children != null) {
        $.each(obj.children, function (j, d) {
            pushSon(d);
        })
    }
}
