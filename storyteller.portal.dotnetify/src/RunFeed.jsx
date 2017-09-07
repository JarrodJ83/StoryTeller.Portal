import React from 'react';
import dotnetify from 'dotnetify';
import { Table } from 'react-bootstrap';
import RunFeedRow from './RunFeedRow';

class RunFeed extends React.Component {
    constructor(props) {
        super(props);
        this.vm = dotnetify.react.connect("RunFeed", this);
        this.state = { Runs:[] };
    }
    componentWillUnmount() {
        this.vm.$destroy();
    }
    shouldComponentUpdate(nextProps) {
        console.log("RunFeed NextProps :: " + nextProps);
        return true;
    }
    render() {  
        return (<Table striped bordered condensed hover>
                <thead>
                <tr>
                    <th>Run Date</th>
                    <th>App</th>
                    <th>Status</th>
                    <th>Results</th>
                </tr>
            </thead>
            <tbody>
                    {this.state.Runs.map(run => <RunFeedRow Run={run} />)}
                    </tbody>
            </Table>  
        );
    }
}
export default RunFeed;