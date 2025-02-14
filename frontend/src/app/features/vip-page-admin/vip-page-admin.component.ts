import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-code-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './vip-page-admin.component.html',
  styleUrls: ['./vip-page-admin.component.scss'],
})
export class VipPageAdminComponent {
  selectedFile: File | null = null;
  uploadStatus: string | null = null;

  constructor(private http: HttpClient) {}

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
    }
  }

  uploadImage(): void {
    if (!this.selectedFile) {
      console.log('No file selected.');
      return;
    }

    const formData = new FormData();
    const title = 'Example Image Title';
    const filename = this.selectedFile.name;

    formData.append('file', this.selectedFile);
    formData.append('title', title);
    formData.append('filename', filename);

    console.log('FormData ready to send:', formData);

    this.http.post('http://localhost:7172/Images', formData).subscribe({
      next: (response) => {
        console.log('Upload response:', response);
        this.uploadStatus = 'Image uploaded successfully!';
        this.selectedFile = null;
      },
      error: (error) => {
        console.error('Error uploading image:', error);
        this.uploadStatus = 'Failed to upload image.';
      },
    });
  }
}
