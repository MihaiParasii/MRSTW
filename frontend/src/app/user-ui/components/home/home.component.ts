import { Component } from '@angular/core'
import { DealCardComponent } from '../deal-card/deal-card.component'
import { DealResponse } from '../../../models/deal/deal-response'
import { DealService } from '../../../services/deal/deal.service'

@Component({
	selector: 'app-home',
	imports: [
		DealCardComponent,
	],
	templateUrl: './home.component.html',
	standalone: true,
	styleUrl: './home.component.css'
})
export class HomeComponent {
	protected deals: Array<DealResponse> = []
	
	constructor(private dealService: DealService) {
		this.dealService.getTop9().subscribe(deals => {
			this.deals = deals
		})
	}
}
