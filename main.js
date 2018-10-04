var app = new Vue({
    el: '#app',
    data: {
        message: 'Hello Vue!',
        todos: []
    },
    methods: {
        someMethod() {
           this.todos = [{text: "fsfsfsf", id: "fsfsfsf"},{text: "fsfsfsf", id: "fsfsfsf"},{text: "fsfsfsf", id: "fsfsfsf"},{text: "fsfsfsf", id: "fsfsfsf"}];
           console.log("fsf" + this.todos);
        }
    },
    created() {
        this.someMethod()
    }
})