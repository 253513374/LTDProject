;class SwitchGoods{
    constructor(parameter) {
        this._box = $('.'+parameter.box_class);
        this._color_group = $('.color-group');
        this.add_event();
    }
    
    add_event(){
        this._box.find('.img-group').hover(function(){
            let this_img = $(this).find('img');
            this_img.css('padding', 0);
        }, function(){
            let this_img = $(this).find('img');
            this_img.css('padding', '2em');
        })
        this._color_group.find('.img-grooup').click(function(){
            let this_class_name = $(this).attr('class');
            if(this_class_name.indexOf('self') < 0){
                //��ʾΪ��ǰʱ��������
                let this_m_img = $(this).attr('data-m-img');
                $(this).parents('.color-group').find('.img-grooup').removeClass('self');
                $(this).addClass('self');
                $(this).parents('.item-group').find('.img-group .goods-img').attr('src', this_m_img);
            }
        });
    }
}