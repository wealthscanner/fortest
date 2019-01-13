import {Routes} from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MyAssetComponent } from './my-asset/my-asset.component';
import { MemberListComponent } from './member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';

export const appRoutes: Routes =  [
    { path: '', component: HomeComponent },
    {
        path: '', runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'allasset', component: MemberListComponent },   /* members */
            { path: 'myasset', component: MyAssetComponent },
            { path: 'messages', component: MessagesComponent }
        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full' }
];
