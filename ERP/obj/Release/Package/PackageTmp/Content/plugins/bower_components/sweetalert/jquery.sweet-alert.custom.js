
!function($) {
    "use strict";

    var SweetAlert = function() {};

    //examples 
    SweetAlert.prototype.init = function() {
        
    //Basic
    $('#sa-basic').click(function(){
        swal("Here's a message!");
    });

    //A title with a text under
    $('#sa-title').click(function(){
        swal("Here's a message!", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed lorem erat eleifend ex semper, lobortis purus sed.")
    });

    //Success Message
    $('#sa-success').click(function(){
        swal("Good job!", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed lorem erat eleifend ex semper, lobortis purus sed.", "success")
        });

    //Saved Message withoud Click
    function SaveMsg() {
        swal("Saved Successfully . . . !", "success");
        }

    //Updated Message withoud Click
    function UpdateMsg() {
        swal("Updated Successfully . . . !", "success");
        }


        var Id = "";
        var Url = "";

        //Warning Message
        void function DeleteModal(Id, Url) {
            swal({
                title: "Are you sure?",
                text: "You will not be able to recover this record . . . !",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, delete it...!",
                closeOnConfirm: false
            }, function (Id, Url) {
                $.ajax({
                    type: "POST",
                    url: Url,
                    dataType: "json",
                    contentType: 'application/json',
                    data: JSON.stringify({ Id: Id }),
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.length > 0) {
                            $.each(data, function (index, obj) {
                                swal("Deleted!", "Your record has been deleted.", "success");
                            });
                        }
                        else {
                            swal("Deleted!", "Something went wrong, Record not deleted . . . !", "danger");
                        }
                    }
                });
            }

    //Warning Message
    $('#sa-warning').click(function(){
        swal({   
            title: "Are you sure?",   
            text: "You will not be able to recover this record . . . !",   
            type: "warning",   
            showCancelButton: true,   
            confirmButtonColor: "#DD6B55",   
            confirmButtonText: "Yes, delete it...!",   
            closeOnConfirm: false 
        }, function(){   
            swal("Deleted!", "Your record has been deleted.", "success"); 
        });
    });

    //Parameter
    $('#sa-params').click(function(){
        swal({   
            title: "Are you sure?",   
            text: "You will not be able to recover this imaginary file!",   
            type: "warning",   
            showCancelButton: true,   
            confirmButtonColor: "#DD6B55",   
            confirmButtonText: "Yes, delete it!",   
            cancelButtonText: "No, cancel plx!",   
            closeOnConfirm: false,   
            closeOnCancel: false 
        }, function(isConfirm){   
            if (isConfirm) {     
                swal("Deleted!", "Your imaginary file has been deleted.", "success");   
            } else {     
                swal("Cancelled", "Your imaginary file is safe :)", "error");   
            } 
        });
        });



    //Custom Image
    $('#sa-image').click(function(){
        swal({   
            title: "Govinda!",   
            text: "Recently joined twitter",   
            imageUrl: "../plugins/images/users/govinda.jpg" 
        });
    });

    //Auto Close Timer
    $('#sa-close').click(function(){
         swal({   
            title: "Auto close alert!",   
            text: "I will close in 2 seconds.",   
            timer: 2000,   
            showConfirmButton: false 
        });
    });


    },
    //init
    $.SweetAlert = new SweetAlert, $.SweetAlert.Constructor = SweetAlert
}(window.jQuery),

//initializing 
function($) {
    "use strict";
    $.SweetAlert.init()
}(window.jQuery);