import {Component, Input} from '@angular/core';
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
  @Input() public deal!: DealResponse;
}
