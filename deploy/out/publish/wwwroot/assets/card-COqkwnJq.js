import{At as e,Bt as t,Dt as n,Ft as r,Rt as i,kt as a,p as o,x as s}from"./index-BWbCubNO.js";var c={name:`Card`,extends:{name:`BaseCard`,extends:o,style:s.extend({name:`card`,style:`
    .p-card {
        background: dt('card.background');
        color: dt('card.color');
        box-shadow: dt('card.shadow');
        border-radius: dt('card.border.radius');
        display: flex;
        flex-direction: column;
    }

    .p-card-caption {
        display: flex;
        flex-direction: column;
        gap: dt('card.caption.gap');
    }

    .p-card-body {
        padding: dt('card.body.padding');
        display: flex;
        flex-direction: column;
        gap: dt('card.body.gap');
    }

    .p-card-title {
        font-size: dt('card.title.font.size');
        font-weight: dt('card.title.font.weight');
    }

    .p-card-subtitle {
        color: dt('card.subtitle.color');
    }
`,classes:{root:`p-card p-component`,header:`p-card-header`,body:`p-card-body`,caption:`p-card-caption`,title:`p-card-title`,subtitle:`p-card-subtitle`,content:`p-card-content`,footer:`p-card-footer`}}),provide:function(){return{$pcCard:this,$parentInstance:this}}},inheritAttrs:!1};function l(o,s,c,l,u,d){return i(),e(`div`,r({class:o.cx(`root`)},o.ptmi(`root`)),[o.$slots.header?(i(),e(`div`,r({key:0,class:o.cx(`header`)},o.ptm(`header`)),[t(o.$slots,`header`)],16)):a(``,!0),n(`div`,r({class:o.cx(`body`)},o.ptm(`body`)),[o.$slots.title||o.$slots.subtitle?(i(),e(`div`,r({key:0,class:o.cx(`caption`)},o.ptm(`caption`)),[o.$slots.title?(i(),e(`div`,r({key:0,class:o.cx(`title`)},o.ptm(`title`)),[t(o.$slots,`title`)],16)):a(``,!0),o.$slots.subtitle?(i(),e(`div`,r({key:1,class:o.cx(`subtitle`)},o.ptm(`subtitle`)),[t(o.$slots,`subtitle`)],16)):a(``,!0)],16)):a(``,!0),n(`div`,r({class:o.cx(`content`)},o.ptm(`content`)),[t(o.$slots,`content`)],16),o.$slots.footer?(i(),e(`div`,r({key:1,class:o.cx(`footer`)},o.ptm(`footer`)),[t(o.$slots,`footer`)],16)):a(``,!0)],16)],16)}c.render=l;export{c as t};