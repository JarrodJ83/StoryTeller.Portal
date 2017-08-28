import React from 'react';
import $ from 'jquery';
window.jQuery = $;
var dotnetify = require('dotnetify');

class HelloWorld extends React.Component {
    constructor(props) {
        super(props);
        dotnetify.react.connect("HelloWorld", this);
        this.state = { Greetings: "", ServerTime: "" };
    }
    render() {
        return (
            <div>
                {this.state.Greetings}<br />
                Server time is: {this.state.ServerTime}
            </div>
        );
    }
}
export default HelloWorld;