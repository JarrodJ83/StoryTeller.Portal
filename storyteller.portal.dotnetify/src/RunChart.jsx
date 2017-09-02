import React, { Component } from 'react';
import dotnetify from 'dotnetify';
import { LineChart, Line, XAxis, YAxis, CartesianGrid } from 'recharts';

class RunChart extends Component {
    constructor(props) {
        super(props);
        dotnetify.react.connect("RunChart", this);
        this.state = { Stats: [] };
    }
    render() {
        return (
            <LineChart width={600} height={300} data={this.state.Stats}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="Day" />
                <YAxis />
                <Line type="monotone" dataKey="SuccessfulCount" stroke="#00FF21" />
                <Line type="monotone" dataKey="FailureCount" stroke="#FF0000" />
            </LineChart>
        );
    }
}

export default RunChart;
