import { Component } from '@angular/core';
import { ImageSelectorService } from 'src/app/shared/components/image-selector/service/image-selector.service';

@Component({
  selector: 'app-vip-page',
  templateUrl: './vip-page.component.html',
  styleUrls: ['./vip-page.component.scss'],
})
export class VipPageComponent {
  isImageSelectorVisible: boolean = false;
  selectedImageUrl?: string;

  constructor(private imageService: ImageSelectorService) {}

  openImageSelector(): void {
    this.isImageSelectorVisible = true;
  }

  closeImageSelector(): void {
    this.isImageSelectorVisible = false;
  }

  ngOnInit() {
    this.imageService.onSelectImage().subscribe({
      next: (response) => {
        this.selectedImageUrl = response.url;
        this.isImageSelectorVisible = false;
      },
    });
  }
}
