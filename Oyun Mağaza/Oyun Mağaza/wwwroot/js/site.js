// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Slider işlevselliği
class Slider {
    constructor(element) {
        this.slider = element;
        this.slides = [
            '/images/featured/zombies-vampires.jpg',
            '/images/featured/weekend-deal.jpg',
            '/images/featured/publisher-sale.jpg'
        ];
        this.currentSlide = 0;
        this.autoPlayInterval = null;

        // Event listeners
        this.slider.querySelector('.prev').addEventListener('click', () => this.prevSlide());
        this.slider.querySelector('.next').addEventListener('click', () => this.nextSlide());

        // Otomatik geçiş başlat
        this.startAutoPlay();

        // Mouse hover olduğunda otomatik geçişi durdur
        this.slider.addEventListener('mouseenter', () => this.stopAutoPlay());
        this.slider.addEventListener('mouseleave', () => this.startAutoPlay());
    }

    showSlide(index) {
        const img = this.slider.querySelector('.slider-content img');
        img.style.opacity = '0';
        
        setTimeout(() => {
            img.src = this.slides[index];
            img.style.opacity = '1';
        }, 200);
    }

    nextSlide() {
        this.currentSlide = (this.currentSlide + 1) % this.slides.length;
        this.showSlide(this.currentSlide);
    }

    prevSlide() {
        this.currentSlide = (this.currentSlide - 1 + this.slides.length) % this.slides.length;
        this.showSlide(this.currentSlide);
    }

    startAutoPlay() {
        if (!this.autoPlayInterval) {
            this.autoPlayInterval = setInterval(() => this.nextSlide(), 5000);
        }
    }

    stopAutoPlay() {
        if (this.autoPlayInterval) {
            clearInterval(this.autoPlayInterval);
            this.autoPlayInterval = null;
        }
    }
}

// Sayfa yüklendiğinde slider'ı başlat
document.addEventListener('DOMContentLoaded', function() {
    const sliderElement = document.querySelector('.featured-slider');
    if (sliderElement) {
        new Slider(sliderElement);
    }
});
