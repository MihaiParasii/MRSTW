import {RouterModule, Routes} from '@angular/router';
import {NgModule} from '@angular/core';
import {HomeComponent} from './user-ui/components/home/home.component';
import {CategoryPageComponent} from './user-ui/components/category-page/category-page.component';
import {DealDetailPageComponent} from './user-ui/components/deal-detail-page/deal-detail-page.component';
import {DealsPageComponent} from './user-ui/components/deals-page/deals-page.component';
import {AuthLayoutComponent} from './login-ui/auth-layout/auth-layout.component';
import {RegisterComponent} from './login-ui/register/register.component';
import {LoginComponent} from './login-ui/login/login.component';
import {LayoutComponent} from './common-ui/components/layout/layout.component';
import {CreateDealRequest} from './models/deal/create-deal-request';
import {AddProductPageComponent} from './user-ui/components/add-product-page/add-product-page.component';
import {SearchPageComponent} from './user-ui/components/search-page/search-page.component';

export const routes: Routes = [
  {
    path: 'auth', component: AuthLayoutComponent, children: [
      {path: 'register', component: RegisterComponent},
      {path: 'login', component: LoginComponent},
    ]
  },


  {
    path: '', component: LayoutComponent, children: [
      {path: '', component: HomeComponent},
      {path: 'add', component: AddProductPageComponent},
      {path: 'search/:query', component: SearchPageComponent},

      {path: 'categories/:category-id', component: CategoryPageComponent},
      {path: ':category-id/:subcategory-id', component: DealsPageComponent},

      {path: ':id', component: DealDetailPageComponent},
    ]
  },

];


@NgModule({
  imports: [RouterModule.forRoot(routes),],
  exports: [RouterModule]
})
export class AppRouteModule {
}
