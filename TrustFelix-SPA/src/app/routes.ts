import {Routes} from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MyAssetComponent } from './my-asset/my-asset.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';

export const appRoutes: Routes =  [
    { path: '', component: HomeComponent },
    {
        path: '', runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'members', component: MemberListComponent },   /* members */
            { path: 'members/:id', component: MemberDetailComponent },   /* /id..Details component */
            { path: 'myasset', component: MyAssetComponent },
            { path: 'messages', component: MessagesComponent }
        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full' }
];
