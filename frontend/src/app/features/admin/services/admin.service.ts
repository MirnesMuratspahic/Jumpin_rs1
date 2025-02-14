import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Chart, registerables } from 'chart.js';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  private dataUrl: string = 'https://localhost:7172/Admin/GetAppInformations';

  constructor(private http: HttpClient) {}

  fetchAppData(): Observable<any[]> {
    return this.http.get<any[]>(this.dataUrl);
  }

  createChart(labels: string[], counts: number[]): Chart {
    Chart.register(...registerables);

    return new Chart('canvas', {
      type: 'bar',
      data: {
        labels: labels,
        datasets: [
          {
            label: 'Broj entiteta',
            data: counts,
            backgroundColor: 'rgba(75, 192, 192, 0.2)',
            borderColor: 'rgba(75, 192, 192, 1)',
            borderWidth: 1,
          },
        ],
      },
      options: {
        responsive: true,
        scales: {
          y: {
            beginAtZero: true,
          },
        },
      },
    });
  }
}
