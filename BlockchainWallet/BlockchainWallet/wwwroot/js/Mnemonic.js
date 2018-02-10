// https://bl.ocks.org/vasturiano/2992bcb530bc2d64519c5b25201492fd

const INIT_DENSITY = 0.00025, // particles per sq px
    PARTICLE_RADIUS_RANGE = [1, 18],
    PARTICLE_VELOCITY_RANGE = [0, 4];

const canvasWidth = window.innerWidth - (window.innerWidth / 4),
    canvasHeight = window.innerHeight - (window.innerWidth / 6),
    svgCanvas = d3.select('svg#canvas')
        .attr('width', canvasWidth)
        .attr('height', canvasHeight);

const forceSim = d3.forceSimulation()
    .alphaDecay(0)
    .velocityDecay(0)
    .on('tick', particleDigest)
    .force('bounce', d3.forceBounce()
        .radius(d => d.r)
    )
    .force('container', d3.forceSurface()
        .surfaces([
            { from: { x: 0, y: 0 }, to: { x: 0, y: canvasHeight } },
            { from: { x: 0, y: canvasHeight }, to: { x: canvasWidth, y: canvasHeight } },
            { from: { x: canvasWidth, y: canvasHeight }, to: { x: canvasWidth, y: 0 } },
            { from: { x: canvasWidth, y: 0 }, to: { x: 0, y: 0 } }
        ])
        .oneWay(true)
        .radius(d => d.r)
    );

// Init particles
onDensityChange(INIT_DENSITY);

// Event handlers
function onDensityChange(density) {
    const newNodes = genNodes(density);
    d3.select('#numparticles-val').text(newNodes.length);
    d3.select('#density-control').attr('value', density);
    forceSim.nodes(newNodes);
}

function onElasticityChange(elasticity) {
    d3.select('#elasticity-val').text(elasticity);
    forceSim.force('bounce').elasticity(elasticity);
    forceSim.force('container').elasticity(elasticity);
}

function genNodes(density) {
    const numParticles = Math.round(canvasWidth * canvasHeight * density),
        existingParticles = forceSim.nodes();

    if (numParticles < existingParticles.length) {
        return existingParticles.slice(0, numParticles);
    }

    return [...existingParticles, ...d3.range(numParticles - existingParticles.length).map(() => {
        const angle = Math.random() * 2 * Math.PI,
            velocity = Math.random() * (PARTICLE_VELOCITY_RANGE[1] - PARTICLE_VELOCITY_RANGE[0]) + PARTICLE_VELOCITY_RANGE[0];

        return {
            x: Math.random() * canvasWidth,
            y: Math.random() * canvasHeight,
            vx: Math.cos(angle) * velocity,
            vy: Math.sin(angle) * velocity,
            r: Math.round(Math.random() * (PARTICLE_RADIUS_RANGE[1] - PARTICLE_RADIUS_RANGE[0]) + PARTICLE_RADIUS_RANGE[0])
        }
    })];
}

function particleDigest() {
    let particle = svgCanvas.selectAll('circle.particle').data(forceSim.nodes().map(hardLimit));

    particle.exit().remove();

    particle.merge(
        particle.enter().append('circle')
            .classed('particle', true)
            .attr('r', d => d.r)
            .attr('fill', '#fcc3a3')
    )
        .attr('cx', d => d.x)
        .attr('cy', d => d.y);
}

function hardLimit(node) {
    // Keep in canvas
    node.x = Math.max(node.r, Math.min(canvasWidth - node.r, node.x));
    node.y = Math.max(node.r, Math.min(canvasHeight - node.r, node.y));

    return node;
} 

$("#Save").click(function () {
    var data = JSON.stringify(svgCanvas.selectAll('circle.particle').data());
    $("#mnemonicData").val(data);
    $("CreateMnemonic").submit();
})