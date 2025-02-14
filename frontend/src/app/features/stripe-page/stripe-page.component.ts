import { Component } from '@angular/core';
import { StripeService } from './service/stripe-service.service';
import { NavbarComponent } from '../../core/components/navbar/navbar.component';
import { routeAd } from '../../constants/imagesConstants';
import { carAd } from '../../constants/imagesConstants';
import { flatAd } from '../../constants/imagesConstants';

declare var Stripe: any;

@Component({
  selector: 'app-stripe-page',
  standalone: true,
  imports: [NavbarComponent],
  templateUrl: './stripe-page.component.html',
  styleUrls: ['./stripe-page.component.scss'],
})
export class StripePageComponent {
  sessionId: any;
  routeAdImage = routeAd;
  carAdimage = carAd;
  flatAdImage = flatAd;

  constructor(private stripeService: StripeService) { }

  async startCheckout() {
    const amount = 20;
    const checkoutSessionData = {
      lineItems: [
        {
          price_data: {
            currency: 'eur',
            product_data: {
              name: 'Product Name',
            },
            unit_amount: 1000,
          },
          quantity: 1,
        }
      ],
      mode: 'payment',
      success_url: `${window.location.origin}/success-page`,
      cancel_url: `${window.location.origin}/cancel-page`,
    };

    try {
      this.stripeService.checkoutSesion(amount).subscribe(
        (data: string) => {
          this.sessionId = data;
          const stripe = Stripe('pk_test_51QcBDlHgp957nrCtbG1aDoJyV051qsFSKhOFXcdTcnwDqrnxeFXQ0e3sFMUlTOJIxv1SlFyyF2reqhs5QwdA5nVE00qQ53Q7PI');
          stripe.redirectToCheckout({
            sessionId: this.sessionId,
          }).then((result:any) => {
            if (result.error) {
              console.error('Stripe Checkout Error: ', result.error.message);
            }
          });
        },
        (error) => {
          console.error('Error during session creation:', error);
        }
      );
    } catch (error) {
      console.error('Error during Checkout session creation:', error);
    }
  }
}
