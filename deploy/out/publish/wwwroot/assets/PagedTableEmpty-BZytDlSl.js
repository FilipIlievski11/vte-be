import{At as e,Bt as t,Dt as n,Ft as r,Mt as i,Pt as a,Rt as o,Zt as s,at as c,en as l,kt as u,p as d,x as f}from"./index-BWbCubNO.js";import{t as p}from"./_plugin-vue_export-helper-BDNMzG2s.js";var m=f.extend({name:`skeleton`,style:`
    .p-skeleton {
        display: block;
        overflow: hidden;
        background: dt('skeleton.background');
        border-radius: dt('skeleton.border.radius');
    }

    .p-skeleton::after {
        content: '';
        animation: p-skeleton-animation 1.2s infinite;
        height: 100%;
        left: 0;
        position: absolute;
        right: 0;
        top: 0;
        transform: translateX(-100%);
        z-index: 1;
        background: linear-gradient(90deg, rgba(255, 255, 255, 0), dt('skeleton.animation.background'), rgba(255, 255, 255, 0));
    }

    [dir='rtl'] .p-skeleton::after {
        animation-name: p-skeleton-animation-rtl;
    }

    .p-skeleton-circle {
        border-radius: 50%;
    }

    .p-skeleton-animation-none::after {
        animation: none;
    }

    @keyframes p-skeleton-animation {
        from {
            transform: translateX(-100%);
        }
        to {
            transform: translateX(100%);
        }
    }

    @keyframes p-skeleton-animation-rtl {
        from {
            transform: translateX(100%);
        }
        to {
            transform: translateX(-100%);
        }
    }
`,classes:{root:function(e){var t=e.props;return[`p-skeleton p-component`,{"p-skeleton-circle":t.shape===`circle`,"p-skeleton-animation-none":t.animation===`none`}]}},inlineStyles:{root:{position:`relative`}}}),h={name:`BaseSkeleton`,extends:d,props:{shape:{type:String,default:`rectangle`},size:{type:String,default:null},width:{type:String,default:`100%`},height:{type:String,default:`1rem`},borderRadius:{type:String,default:null},animation:{type:String,default:`wave`}},style:m,provide:function(){return{$pcSkeleton:this,$parentInstance:this}}};function g(e){"@babel/helpers - typeof";return g=typeof Symbol==`function`&&typeof Symbol.iterator==`symbol`?function(e){return typeof e}:function(e){return e&&typeof Symbol==`function`&&e.constructor===Symbol&&e!==Symbol.prototype?`symbol`:typeof e},g(e)}function _(e,t,n){return(t=v(t))in e?Object.defineProperty(e,t,{value:n,enumerable:!0,configurable:!0,writable:!0}):e[t]=n,e}function v(e){var t=y(e,`string`);return g(t)==`symbol`?t:t+``}function y(e,t){if(g(e)!=`object`||!e)return e;var n=e[Symbol.toPrimitive];if(n!==void 0){var r=n.call(e,t);if(g(r)!=`object`)return r;throw TypeError(`@@toPrimitive must return a primitive value.`)}return(t===`string`?String:Number)(e)}var b={name:`Skeleton`,extends:h,inheritAttrs:!1,computed:{containerStyle:function(){return this.size?{width:this.size,height:this.size,borderRadius:this.borderRadius}:{width:this.width,height:this.height,borderRadius:this.borderRadius}},dataP:function(){return c(_({},this.shape,this.shape))}}},x=[`data-p`];function S(t,n,i,a,s,c){return o(),e(`div`,r({class:t.cx(`root`),style:[t.sx(`root`),c.containerStyle],"aria-hidden":`true`},t.ptmi(`root`),{"data-p":c.dataP}),null,16,x)}b.render=S;var C={class:`empty-state`},w={class:`empty-title`},T={key:0,class:`empty-hint`},E=p(a({__name:`PagedTableEmpty`,props:{icon:{},title:{},hint:{},ctaLabel:{}},emits:[`cta`],setup(r){return(a,c)=>(o(),e(`div`,C,[n(`i`,{class:s([`pi`,r.icon??`pi-inbox`])},null,2),n(`div`,w,l(r.title??`Нема записи`),1),r.hint?(o(),e(`div`,T,l(r.hint),1)):u(``,!0),t(a.$slots,`default`,{},()=>[r.ctaLabel?(o(),e(`button`,{key:0,class:`empty-cta`,onClick:c[0]||=e=>a.$emit(`cta`)},[c[1]||=n(`i`,{class:`pi pi-plus`},null,-1),i(` `+l(r.ctaLabel),1)])):u(``,!0)],!0)]))}}),[[`__scopeId`,`data-v-4957d0a5`]]);export{b as n,E as t};