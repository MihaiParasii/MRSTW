import {RouterModule, Routes} from '@angular/router';
import {NgModule} from '@angular/core';
import {HomeComponent} from './user-ui/components/home/home.component';
import {CategoryPageComponent} from './user-ui/components/category-page/category-page.component';
import {DealDetailPageComponent} from './user-ui/components/deal-detail-page/deal-detail-page.component';

export const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: ':id', component: DealDetailPageComponent},

  {path: 'categories/:id', component: CategoryPageComponent}

];


@NgModule({
  imports: [RouterModule.forRoot(routes),],
  exports: [RouterModule]
})
export class AppRouteModule {
}
