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
import {AddProductPageComponent} from './user-ui/components/add-product-page/add-product-page.component';
import {SearchPageComponent} from './user-ui/components/search-page/search-page.component';
import {AdminLayoutComponent} from './admin-ui/components/admin-layout/admin-layout/admin-layout.component';
import {AdminPageComponent} from './admin-ui/components/admin-ui/admin-page/admin-page.component';
import {
  AdminAddCategoryComponent
} from './admin-ui/components/admin-ui/admin-add-category/admin-add-category.component';
import {
  AdminAddSubcategoryComponent
} from './admin-ui/components/admin-ui/admin-add-subcategory/admin-add-subcategory.component';
import {AdminCategoriesComponent} from './admin-ui/components/admin-ui/admin-categories/admin-categories.component';
import {
  AdminSubcategoriesComponent
} from './admin-ui/components/admin-ui/admin-subcategories/admin-subcategories.component';
import {
  AdminUpdateCategoryComponent
} from './admin-ui/components/admin-ui/admin-update-category/admin-update-category.component';
import {
  AdminUpdateSubcategoryComponent
} from './admin-ui/components/admin-ui/admin-update-subcategory/admin-update-subcategory.component';

export const routes: Routes = [
  {
    path: 'auth', component: AuthLayoutComponent, children: [
      {path: 'register', component: RegisterComponent},
      {path: 'login', component: LoginComponent},
    ]
  },

  {
    path: 'admin', component: AdminLayoutComponent, children: [
      {path: '', component: AdminPageComponent},

      {path: 'categories', component: AdminCategoriesComponent},
      {path: 'addCategory', component: AdminAddCategoryComponent},
      {path: 'updateCategory', component: AdminUpdateCategoryComponent},

      {path: 'subcategories', component: AdminSubcategoriesComponent},
      {path: 'addSubcategory', component: AdminAddSubcategoryComponent},
      {path: 'updateSubcategory', component: AdminUpdateSubcategoryComponent},

      {path: 'users', component: AdminSubcategoriesComponent},
      {path: 'addUser', component: AdminAddSubcategoryComponent},
    ]
  },

  {
    path: '', component: LayoutComponent, children: [
      {path: '', component: HomeComponent},
      {path: 'add', component: AddProductPageComponent},
      {path: 'search/:query', component: SearchPageComponent},

      {path: 'categories/:categoryId', component: CategoryPageComponent},
      {path: ':categoryId/:subcategoryId', component: DealsPageComponent},

      {path: 'deals/:id', component: DealDetailPageComponent},
      {path: 'deals/:categoryId/:subcategoryId', component: DealsPageComponent},
    ]
  },
];


@NgModule({
  imports: [RouterModule.forRoot(routes),],
  exports: [RouterModule]
})
export class AppRouteModule {
}
