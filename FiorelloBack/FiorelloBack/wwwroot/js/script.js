$(document).ready(function () {
    let skip = 8;
    let proCount = $("#count").val();
    $(document).on('click', '#loadMore', function () {

        $.ajax({
            url: '/Product/LoadMore?skip='+skip,
            type: 'GET',
            success: function (res) {
                console.log(res)
                $("#productList").append(res)
                skip += 8;
                if (skip >= proCount) {
                    $('#loadMore').remove();
                }
                //for (let pro of res) {
                //    let mainDiv = $('<div>').addClass("col-sm-6 col-md-4 col-lg-3 mt-3");
                //    let proDiv = $('<div>').addClass("product-item text-center");

                //    let imgDiv = $('img').addClass("img");
                //    let a = $("<a>");
                //    let img = $("img").attr("src", "/img/" + pro, image).addClass("img-fluid");
                //    a.append(img);
                //    imgDiv.append(a);

                //    let divTitle = $("<div>").addClass("title mt-3");
                //    let h6 = $("<h6>").text(pro.title);
                //    divTitle.append(h6);

                //    let divPrice = $("<div>").addClass("price");
                //    let spanCard = $("<span>").addClass("text-black-50").text("Add to cart");
                //    let spanPrice = $("<span>").addClass("text-black-50").text("$" + pro.price);
                //    divPrice.append(imgDiv, divTitle, divPrice);
                //    mainDiv.append(proDiv);
                //    $("#productList").append(mainDiv)
                //}
            }
        })
    })
    // HEADER

    $(document).on('click', '#search', function () {
        $(this).next().toggle();
    })

    $(document).on('click', '#mobile-navbar-close', function () {
        $(this).parent().removeClass("active");

    })
    $(document).on('click', '#mobile-navbar-show', function () {
        $('.mobile-navbar').addClass("active");

    })

    $(document).on('click', '.mobile-navbar ul li a', function () {
        if ($(this).children('i').hasClass('fa-caret-right')) {
            $(this).children('i').removeClass('fa-caret-right').addClass('fa-sort-down')
        }
        else {
            $(this).children('i').removeClass('fa-sort-down').addClass('fa-caret-right')
        }
        $(this).parent().next().slideToggle();
    })

    // SLIDER

    $(document).ready(function(){
        $(".slider").owlCarousel(
            {
                items: 1,
                loop: true,
                autoplay: true
            }
        );
      });

    // PRODUCT

    $(document).on('click', '.categories', function(e)
    {
        e.preventDefault();
        $(this).next().next().slideToggle();
    })

    $(document).on('click', '.category li a', function (e) {
        e.preventDefault();
        let category = $(this).attr('data-id');
        let products = $('.product-item');
        
        products.each(function () {
            if(category == $(this).attr('data-id'))
            {
                $(this).parent().fadeIn();
            }
            else
            {
                $(this).parent().hide();
            }
        })
        if(category == 'all')
        {
            products.parent().fadeIn();
        }
    })

    // ACCORDION 

    $(document).on('click', '.question', function()
    {   
       $(this).siblings('.question').children('i').removeClass('fa-minus').addClass('fa-plus');
       $(this).siblings('.answer').not($(this).next()).slideUp();
       $(this).children('i').toggleClass('fa-plus').toggleClass('fa-minus');
       $(this).next().slideToggle();
       $(this).siblings('.active').removeClass('active');
       $(this).toggleClass('active');
    })

    // TAB

    $(document).on('click', 'ul li', function()
    {   
        $(this).siblings('.active').removeClass('active');
        $(this).addClass('active');
        let dataId = $(this).attr('data-id');
        $(this).parent().next().children('p.active').removeClass('active');

        $(this).parent().next().children('p').each(function()
        {
            if(dataId == $(this).attr('data-id'))
            {
                $(this).addClass('active')
            }
        })
    })

    $(document).on('click', '.tab4 ul li', function()
    {   
        $(this).siblings('.active').removeClass('active');
        $(this).addClass('active');
        let dataId = $(this).attr('data-id');
        $(this).parent().parent().next().children().children('p.active').removeClass('active');

        $(this).parent().parent().next().children().children('p').each(function()
        {
            if(dataId == $(this).attr('data-id'))
            {
                $(this).addClass('active')
            }
        })
    })

    // INSTAGRAM

    $(document).ready(function(){
        $(".instagram").owlCarousel(
            {
                items: 4,
                loop: true,
                autoplay: true,
                responsive:{
                    0:{
                        items:1
                    },
                    576:{
                        items:2
                    },
                    768:{
                        items:3
                    },
                    992:{
                        items:4
                    }
                }
            }
        );
      });

      $(document).ready(function(){
        $(".say").owlCarousel(
            {
                items: 1,
                loop: true,
                autoplay: true
            }
        );
      });
})