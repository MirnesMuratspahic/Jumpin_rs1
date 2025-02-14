import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { GoogleMapsComponent } from './features/google-maps/google-maps.component';

@Component({
    selector: 'app-root',
    imports: [RouterOutlet, ToastrModule],
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Jumpin';
}
