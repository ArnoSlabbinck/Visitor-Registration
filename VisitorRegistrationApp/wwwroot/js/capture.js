const webcamElement = document.getElementById('webcam');
const canvasElement = document.getElementById('canvas');
const snapSoundElement = document.getElementById('snapSound');
const webcam = new Webcam(webcamElement, 'user', canvasElement, snapSoundElement);
const pictureTitle = document.getElementById("Title");
var pictureUrl;
var countDown = document.getElementById("counter");
var h3Element = document.createElement("h3");
var profileInfo = document.querySelector(".picture-intro");
var pictureGuide = document.getElementById("guide-picture");
h3Element.innerHTML = "Be Ready. The Camera will start right away. Smile at the camera";
var h4Element = document.createElement("h4");
h4Element.innerHTML = "Is this snapshot good enough or do you want to try again?";




function SetupCamera() {

    pictureTitle.appendChild(h3Element);

    setTimeout(function () {
        h3Element.style.display = "none";
        pictureTitle.appendChild(h4Element);


    }, 5000);

    startCountDown(5);

}


function startCountDown(seconds) {
    let counter = seconds;


    const interval = setInterval(() => {
        countDown.innerHTML = counter;
        counter--;

        if (counter <= 0) {

            clearInterval(interval);
            TakePicture();
            pictureGuide.style.display = "none";
        }

    }, 1000);

    return true;

}

$("#webcam-start").click(function () {

    $('.md-modal').addClass('md-show');
    removeCapture();
    webcam.start()
        .then(result => {
            document.getElementById("picture-intro").style.display = "none";
            cameraStarted();
            console.log("webcam started");
            SetupCamera();
        })
        .catch(err => {
            console.log(err);
        });


});


function TakePicture() {
    beforeTakePhoto();
    let picture = webcam.snap();
    pictureUrl = picture
    document.querySelector('#download-photo').href = picture;
    afterTakePhoto();
}

function stopWebcam() {
    // Delete canvas
    webcam.stop();
    ctx.clearRect(0, 0, canvas.width, canvas.height);
}

function resizedataURL(datas, wantedWidth, wantedHeight) {
    var img = document.createElement('img');

    img.onload = function () {
        var canvas = document.createElement('');
        var ctx = canvas.getContext('2d');
        canvas.width = wantedWidth;
        canvas.height = wantedHeight;
        ctx.drawImage(this, 0, 0, wantedWidth, wantedHeight);
        var dataURI = canvas.toDataURL();
        img.src = dataURI;
        return dataURI;
    };



}


function cameraStarted() {
    $("#errorMsg").addClass("d-none");
    $('.flash').hide();
    $("#webcam-caption").html("on");
    $("#webcam-control").removeClass("webcam-off");
    $("#webcam-control").addClass("webcam-on");
    $(".webcam-container").removeClass("d-none");
    if (webcam.webcamList.length > 1) {
        $("#cameraFlip").removeClass('d-none');
    }
    $("#wpfront-scroll-top-container").addClass("d-none");
    window.scrollTo(0, 0);
    $('body').css('overflow-y', 'hidden');
}


function cameraStopped() {
    $("#errorMsg").addClass("d-none");
    $("#wpfront-scroll-top-container").removeClass("d-none");
    $("#webcam-control").removeClass("webcam-on");
    $("#webcam-control").addClass("webcam-off");
    $("#cameraFlip").addClass('d-none');
    $(".webcam-container").addClass("d-none");
    $("#webcam-caption").html("Click to Start Camera");
    $('.md-modal').removeClass('md-show');
}

$("#take-photo").click(function () {
    beforeTakePhoto();
    let picture = webcam.snap();
    pictureUrl = picture;
    document.querySelector('#download-photo').href = picture;
    afterTakePhoto();
});

function beforeTakePhoto() {
    $('.flash')
        .show()
        .animate({ opacity: 0.3 }, 500)
        .fadeOut(500)
        .css({ 'opacity': 0.7 });
    window.scrollTo(0, 0);
    $('#webcam-control').addClass('d-none');
    $('#cameraControls').addClass('d-none');
}

function afterTakePhoto() {
    webcam.stop();
    $('#canvas').removeClass('d-none');
    $('#take-photo').addClass('d-none');
    $('#exit-app').removeClass('d-none');
    $('#download-photo').removeClass('d-none');
    $('#resume-camera').removeClass('d-none');
    $('#cameraControls').removeClass('d-none');
}

$("#resume-camera").click(function () {
    h4Element.style.display = "none";
    h3Element.style.display = "block";

    webcam.stream()
        .then(facingMode => {
            removeCapture();
        });
    SetupCamera();
});

$("#exit-app").click(function () {
    // Afsluiten van Modal
    cameraStopped();
    webcam.stop();
    document.getElementById("picture-intro").style.display = "block";
    console.log("webcam stopped");
    const base64 = pictureUrl.split(",")[1];
    console.log(base64);
    var data = {
        DateImage: new Date().toISOString(),
        ImageUrl: base64
    };
    console.log(data);
    console.log(JSON.stringify(data));
    var url = '/Home/RedirectToVisitorProfile';
    

    // Ajax request: redirect doen server naar andere pagina
     fetch(url, {
        method: "post",
        body: JSON.stringify(data),
        headers: {
            "Content-Type": "application/json"
        }
    });
    // Dan Url terug krijgen voor redirct doen naar andere pagina 

});
function removeCapture() {
    $('#canvas').addClass('d-none');
    $('#webcam-control').removeClass('d-none');
    $('#cameraControls').removeClass('d-none');
    $('#take-photo').removeClass('d-none');
    $('#exit-app').addClass('d-none');
    $('#download-photo').addClass('d-none');
    $('#resume-camera').addClass('d-none');
}