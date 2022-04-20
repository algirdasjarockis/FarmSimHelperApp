import xml.etree.ElementTree as et
import argparse
import glob
import os.path

parser = argparse.ArgumentParser(description='Picks only relevant xml info from game files and writes into new xml file')
parser.add_argument(dest='dataType', default='fillTypes')
args = parser.parse_args()

outputFile = 'result.xml'

#
# Simplifies fillTypes xml
#
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

#
# Simplifies production xml
#
def processProductions(filename, outputFilename):
    tree = et.parse(filename)
    root = tree.getroot()

    newTree = et.ElementTree(root.find('./productionPoint/productions'))
    newTree.write(outputFilename, short_empty_elements=True, xml_declaration=True, encoding="utf-8")

    print("Content written to %s" % outputFilename)

if args.dataType == 'fillTypes':
    processFillTypes('../data/fillTypes.xml')
elif args.dataType == 'productions':
    for sourceFile in glob.glob('../data/productions/source/*.xml'):
        outputFilePath = '../data/productions/' + os.path.basename(sourceFile)
        processProductions(sourceFile, outputFilePath)
