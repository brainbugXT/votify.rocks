import React from 'react';

class Gage extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      gage: null
    }
  }

  componentDidMount () {
    
    this.state.gage = new JustGage({
        id: 'confidanceGauge',
        value: 0,
        min: 0,
        max: 5,
        title: 'Vote Session '+this.props.title,
        decimals: 1,
        humanFriendlyDecimal: 1,
        noGradient: true,
        counter: true,
        hideInnerShadow: true,
        levelColors: ['#ff0000', '#f9c802', '#a9d70b'],
        relativeGaugeSize: true,
        gaugeWidthScale: 1.4,
        hideMinMax: true
    });
    
  }
  shouldComponentUpdate (nextProps, nextState) {
      if(nextProps.voteValue > 0){
        nextState.gage.refresh(nextProps.value);
      }
      return false;
  }

  render() {    
    return (
      <div id="confidanceGauge" ></div>
    );
  }
}

export default Gage