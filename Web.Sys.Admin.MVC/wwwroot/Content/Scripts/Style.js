
jQuery(document).ready(function () {

    $(document.body).append('<div class="fakeloader"></div>');//页面加载前滚动
    $(".fakeloader").fakeLoader({//页面加载前滚动
        timeToHide: 500,
        bgColor: "#34495e",
        spinner: "spinner2"
    });
    FormStripes();//表单样式
})
function FormStripes() { //表单样式

    $(".stripes tr").mouseover(function () {
        //如果鼠标移到class为stripe的表格的tr上时，执行函数 
        $(this).addClass("over");
    }).mouseout(function () {
        //给这行添加class值为over，并且当鼠标一出该行时执行函数 
        $(this).removeClass("over");
    }) //移除该行的class 
    $(".stripes tr:even").addClass("alt");
    //给class为stripe的表格的偶数行添加class值为alt 

    $(".textbox,.areabox").focus(function () {
        //如果鼠标移到class为stripe的表格的tr上时，执行函数 
        $(this).addClass("textbox-focused");
    }).blur(function () {
        //给这行添加class值为over，并且当鼠标一出该行时执行函数 
        $(this).removeClass("textbox-focused");
    }) //移除该行的class 
}

