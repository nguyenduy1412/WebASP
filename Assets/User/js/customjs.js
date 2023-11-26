$(document).ready(function () {
    $('#chooseButton').click(function () {
        $('#imageInput').click();
    });

    $('#imageInput').change(function () {
        var file = this.files[0];
        if (file) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#imgCus').attr('src', e.target.result);
                $('#imgCus').show();
            };
            reader.readAsDataURL(file);
        }
    });
    $('#showAcc').click(function () {
        $('#menuShow').toggleClass("show");
    });
    $('.pricebook').each(function () {
        var originalPrice = parseFloat($(this).text());

        if (!isNaN(originalPrice)) {
            var formattedPrice = formatPrice(originalPrice);
            $(this).text(formattedPrice);
        }
    });
    function formatPrice(price) {
        if (price >= 1000) {
            return (price / 1000).toFixed(0) + "k";
        } else {
            return price.toFixed(0);
        }
    }
    function toatsjs(messege) {
        $('.toast-body').text(messege)
        $('#liveToast').toast('show');
    }
    function caculatorMoney() {
        let total = 0;
        $('.product-subtotal').each(function () {
            console.log("alo")
            // Lấy giá trị văn bản của mỗi phần tử và chuyển đổi nó thành số
            var value = parseFloat($(this).text().replace(/[\.,]/g, ''));

            // Kiểm tra xem giá trị có phải là số hợp lệ hay không
            if (!isNaN(value)) {
                // Cộng giá trị vào tổng
                total += value;
            }
        });
        $('.sumMoney').text(total.toLocaleString('vi-VN') + ' vnđ')
    }
    $('.delete-item').click(function (e) {
        e.preventDefault();
        let item = $(this).attr('id');
        var parentTr = $(this).closest('tr');

        // Xóa đi phần tử cha <tr>
        parentTr.remove();
        $.ajax({
            type: 'POST',
            url: `/Ajax/DeleteItem/${item}`, // Loại bỏ dấu ngoặc đơn xung quanh biến
            cache: false,
            timeout: 60000, // Đã sửa thành 60000 (milliseconds)
            success: function (data) {
                console.log("Thành công")
                
                caculatorMoney();
                toatsjs("Xóa thành công")
            },
            error: function (e) {
                console.log("Lỗi: " + e);
            }
        });
    });
    $('.add-to-card').click(function () {
        let bookId = $(this).attr('id');
        console.log(bookId)
        $.ajax({
            type: 'POST',
            url: `/Ajax/AddItem/${bookId}`, // Loại bỏ dấu ngoặc đơn xung quanh biến
            cache: false,
            timeout: 60000, // Đã sửa thành 60000 (milliseconds)
            success: function (data) {
                console.log("Thành công")
                toatsjs("Thêm thành công")
            },
            error: function (e) {
                console.log("Lỗi: " + e);
            }
        });
    });
    $('.ip').change(function (e) {
        let value = e.target.value;
        let cartItemId = e.target.id;
        let price = $(this).closest('tr').find('.amount').text();
        let total = parseFloat(price.replace('.', '')) * parseInt(value);
       
        $(this).closest('tr').find('.product-subtotal').text(total.toLocaleString('vi-VN') + ' đ');
        let data = { quantity: parseInt(value), id: cartItemId }
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: `/Ajax/UpdateCart`,
            data: JSON.stringify(data),
            dataType: 'json',
            cache: false,
            timeout: 600000,
            success: function (data, status) {
                console.log("SUCCESS : ", status);
                caculatorMoney();
                
            },
            error: function (e) {
                console.log("ERROR : ", e);
            }
        });

    })
    $('.delete-favourite').click(function () {
      
        let id = $(this).attr('id');

        console.log(id)
        // Xóa đi phần tử cha <tr>
        var parentTr = $(this).closest('tr');

        // Xóa đi phần tử cha <tr>
        parentTr.remove();
        $.ajax({
            type: 'POST',
            url: `/Ajax/DeleteFavourite/${id}`, // Loại bỏ dấu ngoặc đơn xung quanh biến
            cache: false,
            timeout: 60000, // Đã sửa thành 60000 (milliseconds)
            success: function (data) {
                console.log("Thành công")

              
                toatsjs("Xóa thành công")
            },
            error: function (e) {
                console.log("Lỗi: " + e);
            }
        });
    });
    
});


