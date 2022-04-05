import xml.etree.ElementTree as et
import argparse

parser = argparse.ArgumentParser(description='Picks only relevant xml info from game files and writes into new xml file')
parser.add_argument(dest='dataType', default='fillTypes')
args = parser.parse_args()

outputFile = 'result.xml'

def processFillTypes(filename):
    tree = et.parse(filename)
    root = tree.getroot()

    newRoot = et.Element("fillTypes")

    for fillType in root.findall('./fillType'):
        if str(fillType.attrib['showOnPriceTable']) == "false":
            continue

        newFillTypeEl = et.SubElement(newRoot, "fillType")
        newFillTypeEl.attrib['name'] = str(fillType.attrib['name'])

        for item in fillType.findall('economy'):
            newEconomyEl = et.SubElement(newFillTypeEl, "economy")
            newEconomyEl.attrib['pricePerLiter'] = str(item.attrib['pricePerLiter'])
            newFactorsEl = et.SubElement(newEconomyEl, "factors")

            for factor in item.findall('factors/factor'):
                newFactorEl = et.SubElement(newFactorsEl, "factor")
                newFactorEl.attrib['period'] = str(factor.attrib['period'])
                newFactorEl.attrib['value'] = str(factor.attrib['value'])

    newTree = et.ElementTree(newRoot)
    newTree.write(outputFile, short_empty_elements=True, xml_declaration=True, encoding="utf-8")

    print("Content written to %s" % outputFile)

if args.dataType == 'fillTypes':
    processFillTypes('../data/fillTypes.xml')

