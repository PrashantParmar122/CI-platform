const slides = document.querySelectorAll(".slide")
const vid = document.querySelectorAll("video.slide")
const totalImgInSlider = 4;
const totalImg = 1;
var counter = 0;

slides.forEach(
    (slider, index) => {
        slider.style.left = `${index * 100 / totalImgInSlider}%`
    }
)

const slideImg = () => {
    slides.forEach(
        (slider) => {
            slider.style.transform = `translateX(-${counter * 100}%)`
        }
    )
}

const goPrev = () => {
    if (counter != 0)
        counter--;
    slideImg();
}

const goNext = () => {
    if (counter != totalImg - totalImgInSlider)
        counter++;
    slideImg();
}

const setImage = () => {
    document.getElementById("demo").src = document.querySelector(".slide.active").src;
}