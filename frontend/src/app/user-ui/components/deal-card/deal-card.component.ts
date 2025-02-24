import {Component} from '@angular/core';
import {DealResponse} from '../../../models/deal/deal-response';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-deal-card',
  imports: [
    RouterLink
  ],
  templateUrl: './deal-card.component.html',
  standalone: true,
  styleUrl: './deal-card.component.css'
})
export class DealCardComponent {
  protected deal = new DealResponse();

  constructor() {
    this.deal.Id = 1;
    this.deal.Name = "Test name";
    this.deal.Description = "Test description";
  }
}
