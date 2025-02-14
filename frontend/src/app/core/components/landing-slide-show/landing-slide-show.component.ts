import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-landing-slide-show',
  imports: [CommonModule],
  standalone: true,
  templateUrl: './landing-slide-show.component.html',
  styleUrls: ['./landing-slide-show.component.scss'],
})
export class LandingSlideShowComponent {
  slides = [
    {
      id: 1,
      imageUrl:
        'https://www.shutterstock.com/image-photo/happy-young-latin-business-man-600nw-2424450597.jpg',
      caption: 'Tražiš ili nudiš prevoz?',
      description:
        'Jumpin aplikacija omogućava brzu i jednostavnu pretragu ili ponudu prevoza u tvom pravcu.',
      title: 'Smanji troškove putujući s nama',
    },
    {
      id: 2,
      imageUrl:
        'https://media.istockphoto.com/id/1326353127/photo/estate-agent-showround.jpg?s=612x612&w=0&k=20&c=R7fB1IKc1QoscHbWe3QAEYYLrJOJqVPD4SUvNh5_s0g=',
      caption: 'Tražiš ili nudiš stan za najam?',
      description:
        'Pretraga stanova za najam nikada nije bila lakša. Nađi savršen stan za svoje potrebe!',
      title: 'Pronalazak savršenog vozila za Vas',
    },
    {
      id: 3,
      imageUrl:
        'https://roadcrete.gr/wp-content/uploads/2024/05/types-of-rental-cars-sea-1024x683.jpg',
      caption: 'Rentaš ili si u potrazi za rentom automobila?',
      description:
        'Izaberi auto koji ti najviše odgovara za rentanje na Jumpin aplikaciji, jednostavno i brzo.',
      title: 'Jednostavan pronalazak smještaja',
    },
  ];

  currentSlideIndex = 0;
  slideInterval: any;

  ngOnInit() {
    this.startAutoSlide();
  }

  ngOnDestroy() {
    if (this.slideInterval) {
      clearInterval(this.slideInterval);
    }
  }

  startAutoSlide() {
    this.slideInterval = setInterval(() => {
      this.nextSlide();
    }, 3000);
  }

  prevSlide() {
    if (this.currentSlideIndex > 0) {
      this.currentSlideIndex--;
    } else {
      this.currentSlideIndex = this.slides.length - 1;
    }
  }

  nextSlide() {
    if (this.currentSlideIndex < this.slides.length - 1) {
      this.currentSlideIndex++;
    } else {
      this.currentSlideIndex = 0;
    }
  }
}
