import { CommonModule } from '@angular/common';
import { Component, OnInit, Input } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { dtoRequest } from '../routes/models/dtoRequest.model';
import { FlatsService } from './services/flats.service';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { UserFlat } from '../../../features/home-page/models/userFlat.model';
import { ToastrService } from 'ngx-toastr';
import { RequestsPageService } from '../../../features/requests-page/services/requests-page.service';
import { VoiceRecognitionService } from '../../../services/voiceService/voice-service.service';
import { NgZone } from '@angular/core';

@Component({
  selector: 'app-flats',
  imports: [CommonModule, FormsModule],
  standalone: true,
  templateUrl: './flats.component.html',
  styleUrls: ['./flats.component.scss'],
})
export class FlatsComponent implements OnInit {
  @Input() apiUrl!: string;

  flats: any[] = [];
  filteredFlats: any[] = [];
  response: string = '';
  userEmail: string = '';
  searchTerm: string = '';
  isApiRequestInProgress = false;
  isListening = false;

  newFlat = {
    email: '',
    name: '',
    dateAndTime: '',
    price: 0,
    description: '',
    type: '',
  };

  newRequest: dtoRequest = {
    passengerEmail: '',
    description: '',
    status: '',
    objectId: 0,
    objectType: '',
  };

  constructor(
    private flatService: FlatsService,
    private router: Router,
    private authService: AuthService,
    private toastr: ToastrService,
    private requestService: RequestsPageService,
    private voiceService: VoiceRecognitionService,
    private ngZone: NgZone
  ) {}

  ngOnInit(): void {
    const currentUserEmail = this.authService.getEmailFromToken();

    if (currentUserEmail) {
      this.userEmail = currentUserEmail;
    } else {
      console.error('Email not found in currentUser');
      return;
    }

    if (this.apiUrl === 'https://localhost:7172/Flat/GetFlats') {
      this.getFlats();
    } else if (this.apiUrl === 'https://localhost:7172/Flat/GetUserFlats') {
      this.getUserFlats();
    }
  }

  getFlats(): void {
    this.flatService.getFlats().subscribe(
      (data: any[]) => {
        this.flats = data;
        this.filterFlats();
      },
      (error) => {
        console.error('Error fetching flats:', error);
      }
    );
  }

  getUserFlats() {
    this.flatService.getUserFlats(this.userEmail).subscribe(
      (data: UserFlat[]) => {
        this.flats = data;
        this.filteredFlats = data;
      },
      (error) => {
        console.error('Error fetching flats:', error);
      }
    );
  }

  onSearch(): void {
    if (this.searchTerm) {
      this.filteredFlats = this.flats.filter((flat) =>
        flat.flat.name.toLowerCase().includes(this.searchTerm.toLowerCase())
      );
    } else {
      this.filteredFlats = this.flats;
    }
  }

  sendRequest(item: any): void {
    if (this.isApiRequestInProgress) {
      return;
    }
    this.isApiRequestInProgress = true; 
    this.newRequest.passengerEmail =
      this.userEmail != null ? this.userEmail : '';
    this.newRequest.status = 'Pending';
    this.newRequest.objectId = item.flat.id;
    this.newRequest.objectType = 'Flat';
    this.newRequest.description = 'None';

    this.requestService.sendRequest(this.newRequest).subscribe(
      (apiResponse: string) => {
        this.response = apiResponse;
        this.getFlats();
        this.toastr.success(this.response);
      },
      (error) => {
        this.toastr.error(error.error);
      }
    );
    setTimeout(() => {
      this.isApiRequestInProgress = false;
    }, 5000);
  }

  navigateToRating(email: string): void {
    localStorage.setItem('Email', email);
    this.router.navigate(['/rating']);
  }

  submitNewFlat(): void {
    this.flatService.addFlat(this.newFlat).subscribe(
      (apiResponse: string) => {
        this.response = apiResponse;
        this.router.navigate(['/home']);
        this.toastr.success(this.response);
      },
      (error) => {
        this.toastr.error(error);
      }
    );
  }

  onDeleteFlat(flatId: number) {
    this.flatService.deleteFlat(flatId).subscribe(
      (response: string) => {
        this.flats = this.flats.filter((flat) => flat.flat.id !== flatId);
        this.getUserFlats();
        this.toastr.success(response);
      },
      (error) => {
        this.toastr.error(error);
      }
    );
  }

  filterFlats(): void {
    if (this.searchTerm) {
      const keywords = this.searchTerm
        .toLowerCase()
        .replace(/[^a-zA-Z0-9\s-]/g, '')
        .split(' ')
        .filter(keyword => keyword);
  
      this.filteredFlats = this.flats.filter(flat => {
        const carName = flat.flat.name.toLowerCase().replace(/[^a-zA-Z0-9\s-]/g, '')
  
        return keywords.every(keyword => carName.includes(keyword));
      });
    } else {
      this.filteredFlats = [...this.flats];
    }
    this.filterVIPAds();
  }

  startVoiceSearch() {
    this.isListening = true;
    this.voiceService.startListening((transcript) => {
      this.ngZone.run(() => {
        this.searchTerm = transcript;
      });
    });
    setTimeout(() => {
      this.voiceService.stopListening();
      this.isListening = false;
      this.filterFlats();
    }, 5000);
  }

  filterVIPAds(){
    this.filteredFlats = this.filteredFlats.sort((a, b) => {
      if (a.user.accountType === 'VIP' && b.user.accountType !== 'VIP') {
        return -1; 
      } else if (a.user.accountType !== 'VIP' && b.user.accountType === 'VIP') {
        return 1; 
      }
      return 0;
    });
  }

  onEditFlat(flatId: number): void {
    this.router.navigate(['/edit-route', flatId]);
  }
}
