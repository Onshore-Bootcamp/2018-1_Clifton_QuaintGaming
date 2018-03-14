
$('.navbar').affix({
    offset: { top: 100 }
});

//Disallows deleting an administrator client-side.
function adminDelete() {
    alert("You cannot remove an Administrator.\nAssign lower permissions to remove this user.")
}

function userDelete() {
    return confirm("Remove this user?");
}

function gameDelete() {
    return confirm("Remove this game?")
}

function commentDelete() {
    return confirm("Remove this comment?")
}

//Hides the elements contained in the parent blocks of the selector, and reveals other buttons.
$(function () {
    var $lastClicked = null;
    var $lastForm = null;

    $(".edit-button").click(function () {
        let $this = $(this);
        let $editHide = $this.parent().parent().parent().find(".editHide");
        let $commentEdit = $this.parent().parent().parent().find(".commentEdit");

        $lastClicked = $editHide;
        $lastForm = $commentEdit;
        $editHide.hide();
        $commentEdit.show();
    });
    $(".close-button").click(function () {
        $lastClicked.show();
        $lastForm.hide();
    });
});

//$(document).ready(function () {
//    $('#commentText').blur(function () {
//        if ($.trim(this.value) == "") {
//            $('#commentButton').attr("disabled", true);
//        }
//        else {
//            $('#commentButton').removeAttr("disabled");
//        }
//    });
//});
